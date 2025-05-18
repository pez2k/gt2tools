using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GT2.ModelTool.Structures
{
    using ExportMetadata;
    using StreamExtensions;

    public class Model
    {
        private ushort menuFrontWheelRadius;
        private ushort menuFrontWheelWidth;
        private ushort menuRearWheelRadius;
        private ushort menuRearWheelWidth;
        private ushort lod0MaxDistance; // default 5, transformed to 400 in RAM
        private ushort lod1MaxDistance; // default 15, transformed to 3600 in RAM
        private ushort lod2MaxDistance; // default 300, transformed to 1440000 in RAM

        public List<WheelPosition> WheelPositions { get; set; } = new List<WheelPosition>(4);
        public List<LOD> LODs { get; set; }
        public Shadow Shadow { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            stream.Position = 0x08;
            menuFrontWheelRadius = stream.ReadUShort();
            if (menuFrontWheelRadius == 0) {
                stream.Position = 0x18;
                menuFrontWheelRadius = stream.ReadUShort();
            }

            menuFrontWheelWidth = stream.ReadUShort();
            menuRearWheelRadius = stream.ReadUShort();
            menuRearWheelWidth = stream.ReadUShort();

            for (int i = 0; i < 4; i++) {
                var wheelPosition = new WheelPosition();
                wheelPosition.ReadFromCDO(stream);
                WheelPositions.Add(wheelPosition);
            }

            stream.Position += 0x828;
            uint lodCount = stream.ReadUInt();
            LODs = new List<LOD>((int)lodCount);

            stream.Position += 2; // 2b of zeros, this and the following 2b are replaced in RAM with some near-exponential value calculated from the LOD max distance
            lod0MaxDistance = stream.ReadUShort();
            uint lod0Offset = stream.ReadUInt(); // file offset for start of LOD0 data, becomes a pointer in RAM - always 0 instead of the real value
            stream.Position += 2;
            lod1MaxDistance = stream.ReadUShort();
            uint lod1Offset = stream.ReadUInt(); // file offset for start of LOD1 data, becomes a pointer in RAM
            stream.Position += 2;
            lod2MaxDistance = stream.ReadUShort();
            uint lod2Offset = stream.ReadUInt(); // file offset for start of LOD2 data, becomes a pointer in RAM

            for (int i = 0; i < lodCount; i++)
            {
                var lod = new LOD();
                lod.ReadFromCDO(stream);
                LODs.Add(lod);
            }
            Shadow = new Shadow();
            Shadow.ReadFromCDO(stream);
            if (stream.Position != stream.Length)
            {
                throw new Exception($"{stream.Length - stream.Position} trailing bytes after shadow");
            }
        }

        public void ReadFromCAR(Stream stream)
        {
            stream.Position = 0x10;

            for (int i = 0; i < 4; i++)
            {
                var wheelPosition = new WheelPosition();
                wheelPosition.ReadFromCAR(stream);
                WheelPositions.Add(wheelPosition);
            }

            WheelPositions = [ WheelPositions[2], WheelPositions[3], WheelPositions[0], WheelPositions[1] ];

            menuFrontWheelRadius = stream.ReadUShort();
            menuFrontWheelWidth = stream.ReadUShort();
            menuRearWheelRadius = stream.ReadUShort();
            menuRearWheelWidth = stream.ReadUShort();

            stream.Position += 0x04;
            ushort lodCount = stream.ReadUShort();
            LODs = new List<LOD>(lodCount);

            stream.Position += 0x42;

            for (int i = 1; i <= lodCount; i++)
            {
                var lod = new LOD();
                lod.ReadFromCAR(stream);
                LODs.Add(lod);
                if (i != lodCount)
                {
                    stream.Position += 40; // gap between LODs
                }
            }
            Shadow = new Shadow();
            Shadow.ReadFromCAR(stream);
            if (stream.Position != stream.Length)
            {
                throw new Exception($"{stream.Length - stream.Position} trailing bytes after shadow");
            }
        }

        public void WriteToCDO(Stream stream)
        {
            if (LODs.Count != 3)
            {
                throw new Exception("CDO requires 3 LODs");
            }

            // GT header
            stream.Write([0x47, 0x54, 0x02]);
            stream.Position = 0x18;
            stream.WriteUShort(menuFrontWheelRadius);
            stream.WriteUShort(menuFrontWheelWidth);
            stream.WriteUShort(menuRearWheelRadius);
            stream.WriteUShort(menuRearWheelWidth);

            foreach (WheelPosition wheelPosition in WheelPositions)
            {
                wheelPosition.WriteToCDO(stream);
            }

            stream.Position = 0x868;
            stream.WriteUInt((uint)LODs.Count);
            stream.WriteUShort(0); // padding before LOD0 max distance
            stream.WriteUShort(lod0MaxDistance);
            stream.WriteUInt(0); // should be LOD0 file offset, but is always 0
            stream.WriteUShort(0); // padding before LOD1 max distance
            stream.WriteUShort(lod1MaxDistance);
            long lod1OffsetPosition = stream.Position;
            stream.WriteUInt(0); // placeholder for LOD1 file offset
            stream.WriteUShort(0); // padding before LOD2 max distance
            stream.WriteUShort(lod2MaxDistance);
            long lod2OffsetPosition = stream.Position;
            stream.WriteUInt(0); // placeholder for LOD2 file offset

            List<long> lodOffsets = [];
            foreach (LOD lod in LODs)
            {
                lodOffsets.Add(stream.Position);
                lod.WriteToCDO(stream);
            }

            long dataPosition = stream.Position;
            stream.Position = lod1OffsetPosition;
            stream.WriteUInt((uint)lodOffsets[1]);
            stream.Position = lod2OffsetPosition;
            stream.WriteUInt((uint)lodOffsets[2]);
            stream.Position = dataPosition;

            Shadow.WriteToCDO(stream);

            if (stream.Position >= 0x5000)
            {
                throw new Exception($"Maximum CDO file size exceeded: {stream.Position} of {0x5000} bytes");
            }
        }

        public void WriteToOBJ(TextWriter modelWriter, TextWriter materialWriter, string filename, ModelMetadata metadata, bool splitOverlappingFaces)
        {
            metadata.MenuWheels.FrontWheelDiameter = menuFrontWheelRadius * 2 * Vertex.UnitsToMetres;
            metadata.MenuWheels.FrontWheelWidth = menuFrontWheelWidth * Vertex.UnitsToMetres;
            metadata.MenuWheels.RearWheelDiameter = menuRearWheelRadius * 2 * Vertex.UnitsToMetres;
            metadata.MenuWheels.RearWheelWidth = menuRearWheelWidth * Vertex.UnitsToMetres;
            metadata.LOD0.MaxDistance = lod0MaxDistance;
            metadata.LOD1.MaxDistance = lod1MaxDistance;
            metadata.LOD2.MaxDistance = lod2MaxDistance;

            modelWriter.WriteLine($"mtllib {filename}.mtl");
            
            int vertexNumber = 1;
            int normalNumber = 1;
            int coordNumber = 1;

            List<short> menuWheelOffsets = [];
            for (int i = 0; i < WheelPositions.Count; i++)
            {
                WheelPositions[i].WriteToOBJ(modelWriter, i, vertexNumber, menuWheelOffsets);
                vertexNumber += 4;
            }

            if (WheelPositions.Count != 4)
            {
                throw new Exception("Expected 4 wheel positions");
            }

            metadata.MenuWheels.FrontLeftXOffset = menuWheelOffsets[0] * Vertex.UnitsToMetres;
            metadata.MenuWheels.FrontRightXOffset = menuWheelOffsets[1] * Vertex.UnitsToMetres;
            metadata.MenuWheels.RearLeftXOffset = menuWheelOffsets[2] * Vertex.UnitsToMetres;
            metadata.MenuWheels.RearRightXOffset = menuWheelOffsets[3] * Vertex.UnitsToMetres;

            metadata.MergeOverlappingFaces = splitOverlappingFaces;
            LODMetadata[] lodMetadata = [ metadata.LOD0, metadata.LOD1, metadata.LOD2 ];
            List<MaterialMetadata> materialMetadata = [];

            for (int i = 0; i < LODs.Count; i++)
            {
                LODs[i].WriteToOBJ(modelWriter, i, vertexNumber, normalNumber, coordNumber, lodMetadata[i], materialMetadata, splitOverlappingFaces);
                vertexNumber += LODs[i].Vertices.Count;
                normalNumber += LODs[i].Normals.Count;
                coordNumber += LODs[i].GetAllUVCoords().Count;
            }

            metadata.Materials = materialMetadata.Distinct().ToArray();

            Shadow.WriteToOBJ(modelWriter, vertexNumber, metadata.Shadow);
            vertexNumber += Shadow.Vertices.Count; // Not strictly needed, but required to stay in sync if the shadow writing is moved before another part

            materialWriter.WriteLine("newmtl untextured");
            materialWriter.WriteLine("Kd 0 0 0");
            materialWriter.WriteLine("newmtl shadow");
            materialWriter.WriteLine("Kd 0 0 0");
            materialWriter.WriteLine("newmtl shadowgradient");
            materialWriter.WriteLine("Kd 0.1 0.1 0.1");

            foreach (MaterialMetadata material in metadata.Materials)
            {
                materialWriter.WriteLine($"newmtl {material.Name}");
                if (material.IsUntextured)
                {
                    int R = material.SolidColour & 0xFF;
                    int G = (material.SolidColour >> 8) & 0xFF;
                    int B = (material.SolidColour >> 16) & 0xFF;
                    materialWriter.WriteLine($"Kd {R} {G} {B}");
                }
                else
                {
                    materialWriter.WriteLine($"map_Kd palette{material.PaletteIndex.Value}.bmp");
                }
            }
        }

        public void ReadFromOBJ(TextReader reader, string directory, ModelMetadata metadata)
        {
            menuFrontWheelRadius = (ushort)(metadata.MenuWheels.FrontWheelDiameter / 2 / Vertex.UnitsToMetres);
            menuFrontWheelWidth = (ushort)(metadata.MenuWheels.FrontWheelWidth / Vertex.UnitsToMetres);
            menuRearWheelRadius = (ushort)(metadata.MenuWheels.RearWheelDiameter / 2 / Vertex.UnitsToMetres);
            menuRearWheelWidth = (ushort)(metadata.MenuWheels.RearWheelWidth / Vertex.UnitsToMetres);
            lod0MaxDistance = metadata.LOD0.MaxDistance;
            lod1MaxDistance = metadata.LOD1.MaxDistance;
            lod2MaxDistance = metadata.LOD2.MaxDistance;

            var lods = new LOD[3];
            LODMetadata[] lodMetadata = [ metadata.LOD0, metadata.LOD1, metadata.LOD2 ];
            var wheelPositions = new WheelPosition[4];
            short[] wheelXOffsets = [
                (short)(metadata.MenuWheels.FrontLeftXOffset / Vertex.UnitsToMetres),
                (short)(metadata.MenuWheels.FrontRightXOffset / Vertex.UnitsToMetres),
                (short)(metadata.MenuWheels.RearLeftXOffset / Vertex.UnitsToMetres),
                (short)(metadata.MenuWheels.RearRightXOffset / Vertex.UnitsToMetres)
            ];
            string line;
            WheelPosition currentWheelPosition = null;
            int currentWheelPositionNumber = -1;
            List<Vertex> wheelPositionVertices = [];
            int currentLODNumber = -1;
            bool shadow = false;
            string currentMaterialName = "untextured";
            MaterialMetadata currentMaterial = null;
            LOD currentLOD = null;
            var vertices = new List<Vertex>();
            var normals = new List<Normal>();
            var uvCoords = new List<UVCoordinate>();
            var usedVertexIDs = new List<int>();
            var usedNormalIDs = new List<int>();
            int shadowVertexStartID = 0;
            double currentScale = 1;
            List<Vertex> currentLODVertices = [];
            Dictionary<Vertex, Vertex> duplicateVertices = [];
            Dictionary<string, int> materialColours = [];
            int lineNumber = 0;
            do
            {
                line = reader.ReadLine();
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                {
                    continue;
                }

                try
                {
                    if (line.StartsWith("mtllib "))
                    {
                        string[] parts = line.Split(' ');
                        if (parts.Length == 2)
                        {
                            string materialFilename = parts[1];
                            string materialFilePath = Path.Combine(directory, materialFilename);
                            if (!File.Exists(materialFilePath))
                            {
                                throw new Exception($"Could not find or access a file named {materialFilename}");
                            }

                            using (TextReader materialReader = new StreamReader(materialFilePath))
                            {
                                materialColours = ReadMTLFile(materialReader);
                            }
                        }
                    }
                    else if (line.StartsWith("o ") || line.StartsWith("g "))
                    {
                        if (currentLOD != null)
                        {
                            currentLOD.ReadFromOBJ(vertices, normals, usedVertexIDs, usedNormalIDs, metadata.MergeOverlappingFaces, duplicateVertices);
                            currentLODVertices.Clear();
                            duplicateVertices.Clear();
                            usedVertexIDs.Clear();
                            usedNormalIDs.Clear();
                        }
                        else if (currentWheelPosition != null)
                        {
                            currentWheelPosition.ReadFromOBJ(wheelPositionVertices, wheelXOffsets[currentWheelPositionNumber]);
                            wheelPositionVertices.Clear();
                        }
                        currentLOD = null;
                        currentLODNumber = -1;
                        currentScale = 1;
                        shadow = false;
                        currentWheelPosition = null;
                        currentWheelPositionNumber = -1;
                        currentMaterial = null;

                        string[] objectNameParts = line.Split(' ')[1].Split('/');
                        string objectName = objectNameParts[0];

                        if (objectName.StartsWith("lod"))
                        {
                            if (!int.TryParse(objectName.Replace("lod", ""), out currentLODNumber))
                            {
                                throw new Exception($"Could not read LOD number from object name '{objectName}'");
                            }
                            currentLOD = new LOD();
                            LODMetadata currentMetadata = lodMetadata[currentLODNumber];
                            currentLOD.PrepareForOBJRead(currentMetadata);
                            lods[currentLODNumber] = currentLOD;
                            currentScale = currentMetadata.Scale;
                        }
                        else if (objectName.StartsWith("shadow"))
                        {
                            shadow = true;
                            Shadow = new Shadow();
                            Shadow.PrepareForOBJRead(metadata.Shadow);
                            shadowVertexStartID = vertices.Count;
                            currentScale = metadata.Shadow.Scale;
                        }
                        else if (objectName.StartsWith("wheelpos"))
                        {
                            if (!int.TryParse(objectName.Replace("wheelpos", ""), out currentWheelPositionNumber))
                            {
                                throw new Exception($"Could not read wheel position number from object name '{objectName}'");
                            }
                            currentWheelPosition = new WheelPosition();
                            wheelPositions[currentWheelPositionNumber] = currentWheelPosition;
                        }
                    }
                    else if (line.StartsWith("v "))
                    {
                        if (shadow)
                        {
                            var vertex = new ShadowVertex();
                            vertex.ReadFromOBJ(line, currentScale);
                            Shadow.Vertices.Add(vertex);
                            vertices.Add(new Vertex()); // Avoid the number of vertices desyncing if the shadow was before another object
                        }
                        else
                        {
                            var vertex = new Vertex();
                            vertex.ReadFromOBJ(line, currentScale);
                            vertices.Add(vertex);
                            currentLODVertices.Add(vertex);

                            if (currentWheelPosition != null)
                            {
                                wheelPositionVertices.Add(vertex);
                            }
                            else if (metadata.MergeOverlappingFaces)
                            {
                                Vertex matchingVertex = currentLODVertices.FirstOrDefault(vertex.IsDuplicateOf);
                                if (matchingVertex != null)
                                {
                                    duplicateVertices.Add(vertex, matchingVertex);
                                }
                            }
                        }
                    }
                    else if (line.StartsWith("vt "))
                    {
                        var uvCoord = new UVCoordinate();
                        uvCoord.ReadFromOBJ(line);
                        uvCoords.Add(uvCoord);
                    }
                    else if (line.StartsWith("vn "))
                    {
                        var normal = new Normal();
                        normal.ReadFromOBJ(line);
                        normals.Add(normal);
                    }
                    else if (line.StartsWith("usemtl "))
                    {
                        currentMaterialName = line.Split(' ')[1];
                        if (currentLOD != null) // Only LODs have material properties
                        {
                            currentMaterial = FindMaterial(currentMaterialName, metadata.Materials, metadata.AllowUnmappedMaterials);
                            if (materialColours.TryGetValue(currentMaterialName, out int materialColour))
                            {
                                currentMaterial.SolidColour = materialColour;
                            }
                        }
                    }
                    else if (line.StartsWith("f "))
                    {
                        if (shadow)
                        {
                            var polygon = new ShadowPolygon();
                            polygon.ReadFromOBJ(line, Shadow.Vertices, shadowVertexStartID, currentMaterialName, metadata.Shadow);
                            if (polygon.IsQuad)
                            {
                                Shadow.Quads.Add(polygon);
                            }
                            else
                            {
                                Shadow.Triangles.Add(polygon);
                            }
                        }
                        else if (currentWheelPosition != null)
                        {
                            // Ignore faces for wheel positions, we only need the vertex positions
                            continue;
                        }
                        else
                        {
                            if (currentMaterial == null)
                            {
                                throw new Exception("No material specified for face");
                            }
                            if (currentMaterial.IsUntextured)
                            {
                                var polygon = new Polygon();
                                polygon.ReadFromOBJ(line, vertices, normals, currentMaterial, usedVertexIDs, usedNormalIDs);
                                if (polygon.IsQuad)
                                {
                                    currentLOD.Quads.Add(polygon);
                                }
                                else
                                {
                                    currentLOD.Triangles.Add(polygon);
                                }
                            }
                            else
                            {
                                var polygon = new UVPolygon();
                                polygon.ReadFromOBJ(line, vertices, normals, uvCoords, currentMaterial, usedVertexIDs, usedNormalIDs);
                                if (polygon.IsQuad)
                                {
                                    currentLOD.UVQuads.Add(polygon);
                                }
                                else
                                {
                                    currentLOD.UVTriangles.Add(polygon);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception($"Line {lineNumber}: {line}\r\n{exception.Message}", exception);
                }
            }
            while (line != null);

            // Finalise any objects that might still be open
            currentLOD?.ReadFromOBJ(vertices, normals, usedVertexIDs, usedNormalIDs, metadata.MergeOverlappingFaces, duplicateVertices);
            currentWheelPosition?.ReadFromOBJ(wheelPositionVertices, wheelXOffsets[currentWheelPositionNumber]);

            if (wheelPositions.Any(position => position == null))
            {
                throw new Exception("One or more of the four wheel position objects are missing");
            }

            if (lods.Any(lod => lod == null))
            {
                throw new Exception("One or more of the three LOD objects are missing");
            }

            if (Shadow == null)
            {
                throw new Exception("The shadow object is missing");
            }

            for (int i = 0; i < 3; i++)
            {
                if (lods[i].Vertices.Count > 256)
                {
                    throw new Exception($"LOD{i} has more than the maximum of 256 vertices");
                }
            }

            if (Shadow.Vertices.Count > 256)
            {
                throw new Exception($"Shadow has more than the maximum of 256 vertices");
            }

            Shadow.GenerateBoundingBox();
            LODs = lods.ToList();
            WheelPositions = wheelPositions.ToList();
        }

        private static Dictionary<string, int> ReadMTLFile(TextReader reader)
        {
            string line;
            int lineNumber = 0;
            string currentMaterialName = null;
            Dictionary<string, int> materialColours = [];
            do
            {
                line = reader.ReadLine();
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                {
                    continue;
                }

                try
                {
                    if (line.StartsWith("newmtl "))
                    {
                        string[] parts = line.Split(' ');
                        if (parts.Length > 1)
                        {
                            currentMaterialName = parts[1];
                        }
                    }
                    else if (line.StartsWith("Kd "))
                    {
                        string[] parts = line.Split(' ');
                        if (parts.Length >= 4 && double.TryParse(parts[1], out double R) && R >= 0 && R < 256
                                              && double.TryParse(parts[2], out double G) && G >= 0 && G < 256
                                              && double.TryParse(parts[3], out double B) && B >= 0 && B < 256)
                        {
                            int diffuseColour = (int)R + ((int)G << 8) + ((int)B << 16);
                            if (diffuseColour > 0 && !string.IsNullOrEmpty(currentMaterialName))
                            {
                                materialColours.Add(currentMaterialName, diffuseColour);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception($"MTL file line {lineNumber}: {line}\r\n{exception.Message}", exception);
                }
            }
            while (line != null);

            return materialColours;
        }

        private static MaterialMetadata FindMaterial(string materialName, MaterialMetadata[] materials, bool allowUnmappedMaterials) =>
            allowUnmappedMaterials ? LoadMaterial(materialName, materials)
                                        ?? ParseMaterialName(materialName)
                                        ?? ParseMaterialNameLegacy(materialName)
                                        ?? throw new Exception($"Could not find or parse material name '{materialName}'")
                                   : LoadMaterial(materialName, materials)
                                        ?? throw new Exception($"Could not find material name '{materialName}' in JSON file");

        private static MaterialMetadata LoadMaterial(string materialName, MaterialMetadata[] materials)
        {
            MaterialMetadata material = materials.Where(material => material.Name == materialName).FirstOrDefault();
            if (material != null)
            {
                if (material.RenderOrder < 0 || material.RenderOrder >= 32)
                {
                    throw new Exception($"Material '{material.Name}' has missing or invalid render order in JSON file");
                }

                if (!material.IsUntextured && (material.PaletteIndex == null || material.PaletteIndex < 0 || material.PaletteIndex >= 16))
                {
                    throw new Exception($"Material '{material.Name}' has missing or invalid palette index in JSON file");
                }
            }
            return material;
        }

        private static MaterialMetadata ParseMaterialName(string materialName)
        {
            if (!materialName.Contains('_'))
            {
                return null;
            }
            MaterialMetadata material = new();
            string[] parts = materialName.Split('_');
            bool foundRenderOrder = false;

            foreach (string part in parts)
            {
                if (part.StartsWith("untextured"))
                {
                    material.PaletteIndex = null;
                    material.IsUntextured = true;
                }
                else if (part.StartsWith("palette") && ushort.TryParse(part.Replace("palette", ""), out ushort paletteIndex) && paletteIndex < 16)
                {
                    material.PaletteIndex = paletteIndex;
                    material.IsUntextured = false;
                }
                else if (part.StartsWith("order") && int.TryParse(part.Replace("order", ""), out int order) && order >= 0 && order < 32)
                {
                    material.RenderOrder = order;
                    foundRenderOrder = true;
                }
                else if (part.StartsWith("brake"))
                {
                    material.IsBrakeLight = true;
                }
                else if (part.StartsWith("matte"))
                {
                    material.IsMatte = true;
                }
            }

            return (material.IsUntextured || material.PaletteIndex != null) && foundRenderOrder ? material : null;
        }

        private static MaterialMetadata ParseMaterialNameLegacy(string materialName)
        {
            if (!materialName.Contains('/'))
            {
                return null;
            }
            MaterialMetadata material = new();
            string[] parts = materialName.Split('/');
            bool foundOrder = false;
            bool foundFlags = false;

            foreach (string part in parts)
            {
                string[] pair = part.Split('=');
                if (part.StartsWith("untextured"))
                {
                    material.PaletteIndex = null;
                    material.IsUntextured = true;
                }
                else if (pair[0] == "palette" && ushort.TryParse(pair[1], out ushort paletteIndex) && paletteIndex < 16)
                {
                    material.PaletteIndex = paletteIndex;
                    material.IsUntextured = false;
                }
                else if (pair[0] == "order" && int.TryParse(pair[1], out int order) && order >= 0 && order < 32)
                {
                    material.RenderOrder = order;
                    foundOrder = true;
                }
                else if (pair[0] == "flags" && int.TryParse(pair[1], out int flags) && flags >= 0 && flags < 13)
                {
                    material.IsBrakeLight = (flags & 4) != 0;
                    material.IsMatte = (flags & 8) == 0;
                    foundFlags = true;
                }
            }
            return (material.IsUntextured || material.PaletteIndex != null) && foundOrder && foundFlags ? material : null;
        }

        private static double GetScale(string[] objectNameParts)
        {
            foreach (string part in objectNameParts)
            {
                string[] pair = part.Split('=');
                if (pair.Length == 2 && pair[0] == "scale")
                {
                    return double.Parse(pair[1]);
                }
            }
            return 1;
        }
    }
}