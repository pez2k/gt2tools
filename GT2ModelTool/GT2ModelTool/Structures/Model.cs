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
        public ushort MenuFrontWheelRadius { get; set; }
        public ushort MenuFrontWheelWidth { get; set; }
        public ushort MenuRearWheelRadius { get; set; }
        public ushort MenuRearWheelWidth { get; set; }
        public List<WheelPosition> WheelPositions { get; set; } = new List<WheelPosition>(4);
        public byte[] UnknownAll { get; set; } = new byte[24];
        public ushort LOD0MaxDistance { get; set; } // default 5, transformed to 400 in RAM
        public uint LOD0Offset { get; set; }
        public ushort LOD1MaxDistance { get; set; } // default 15, transformed to 3600 in RAM
        public uint LOD1Offset { get; set; }
        public ushort LOD2MaxDistance { get; set; } // default 300, transformed to 1440000 in RAM
        public uint LOD2Offset { get; set; }

        public List<LOD> LODs { get; set; }
        public Shadow Shadow { get; set; }

        public void ReadFromCDO(Stream stream) {
            stream.Position = 0x08;
            MenuFrontWheelRadius = stream.ReadUShort();
            if (MenuFrontWheelRadius == 0) {
                stream.Position = 0x18;
                MenuFrontWheelRadius = stream.ReadUShort();
            }

            MenuFrontWheelWidth = stream.ReadUShort();
            MenuRearWheelRadius = stream.ReadUShort();
            MenuRearWheelWidth = stream.ReadUShort();

            for (int i = 0; i < 4; i++) {
                var wheelPosition = new WheelPosition();
                wheelPosition.ReadFromCDO(stream);
                WheelPositions.Add(wheelPosition);
            }

            stream.Position += 0x828;
            uint lodCount = stream.ReadUInt();
            LODs = new List<LOD>((int)lodCount);

            stream.Read(UnknownAll);
            stream.Position -= UnknownAll.Length;

            stream.Position += 2; // 2b of zeros, this and the following 2b are replaced in RAM with some near-exponential value calculated from the LOD max distance
            LOD0MaxDistance = stream.ReadUShort();
            LOD0Offset = stream.ReadUInt(); // file offset for start of LOD0 data, becomes a pointer in RAM - always 0 instead of the real value
            stream.Position += 2;
            LOD1MaxDistance = stream.ReadUShort();
            LOD1Offset = stream.ReadUInt(); // file offset for start of LOD1 data, becomes a pointer in RAM
            stream.Position += 2;
            LOD2MaxDistance = stream.ReadUShort();
            LOD2Offset = stream.ReadUInt(); // file offset for start of LOD2 data, becomes a pointer in RAM

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

            MenuFrontWheelRadius = stream.ReadUShort();
            MenuFrontWheelWidth = stream.ReadUShort();
            MenuRearWheelRadius = stream.ReadUShort();
            MenuRearWheelWidth = stream.ReadUShort();

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
            stream.WriteUShort(MenuFrontWheelRadius);
            stream.WriteUShort(MenuFrontWheelWidth);
            stream.WriteUShort(MenuRearWheelRadius);
            stream.WriteUShort(MenuRearWheelWidth);

            foreach (WheelPosition wheelPosition in WheelPositions)
            {
                wheelPosition.WriteToCDO(stream);
            }

            stream.Position = 0x868;
            stream.WriteUInt((uint)LODs.Count);
            stream.WriteUShort(0); // padding before LOD0 max distance
            stream.WriteUShort(LOD0MaxDistance);
            stream.WriteUInt(0); // should be LOD0 file offset, but is always 0
            stream.WriteUShort(0); // padding before LOD1 max distance
            stream.WriteUShort(LOD1MaxDistance);
            long lod1OffsetPosition = stream.Position;
            stream.WriteUInt(0); // placeholder for LOD1 file offset
            stream.WriteUShort(0); // padding before LOD2 max distance
            stream.WriteUShort(LOD2MaxDistance);
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
        }

        public void WriteToOBJ(TextWriter modelWriter, TextWriter materialWriter, string filename, Stream unknownData, ModelMetadata metadata)
        {
            unknownData.WriteUShort(MenuFrontWheelRadius);
            unknownData.WriteUShort(MenuFrontWheelWidth);
            unknownData.WriteUShort(MenuRearWheelRadius);
            unknownData.WriteUShort(MenuRearWheelWidth);
            unknownData.WriteUShort(0); // the format expects the top 2b of the LOD count here, always 00 00
            unknownData.Write(UnknownAll);

            metadata.MenuFrontWheelRadius = MenuFrontWheelRadius;
            metadata.MenuFrontWheelWidth = MenuFrontWheelWidth;
            metadata.MenuRearWheelRadius = MenuRearWheelRadius;
            metadata.MenuRearWheelWidth = MenuRearWheelWidth;
            metadata.LOD0.MaxDistance = LOD0MaxDistance;
            metadata.LOD1.MaxDistance = LOD1MaxDistance;
            metadata.LOD2.MaxDistance = LOD2MaxDistance;

            modelWriter.WriteLine($"mtllib {filename}.mtl");
            
            int vertexNumber = 1;
            int normalNumber = 1;
            int coordNumber = 1;
            var materialNames = new Dictionary<string, int?>();

            WheelMetadata[] wheelMetadata = [ metadata.WheelFrontLeft, metadata.WheelFrontRight, metadata.WheelRearLeft, metadata.WheelRearRight ];

            for (int i = 0; i < WheelPositions.Count; i++)
            {
                WheelPositions[i].WriteToOBJ(modelWriter, i, vertexNumber, wheelMetadata[i]);
                vertexNumber++;
            }

            LODMetadata[] lodMetadata = [ metadata.LOD0, metadata.LOD1, metadata.LOD2 ];
            List<MaterialMetadata> materialMetadata = [];

            for (int i = 0; i < LODs.Count; i++)
            {
                LODs[i].WriteToOBJ(modelWriter, i, vertexNumber, normalNumber, coordNumber, materialNames, unknownData, lodMetadata[i], materialMetadata);
                vertexNumber += LODs[i].Vertices.Count;
                normalNumber += LODs[i].Normals.Count;
                coordNumber += LODs[i].GetAllUVCoords().Count;
            }

            metadata.Materials = materialMetadata.Distinct().ToArray();

            Shadow.WriteToOBJ(modelWriter, vertexNumber, unknownData, metadata.Shadow);
            vertexNumber += Shadow.Vertices.Count; // Not strictly needed, but required to stay in sync if the shadow writing is moved before another part

            materialWriter.WriteLine("newmtl untextured");
            materialWriter.WriteLine("Kd 0 0 0");
            materialWriter.WriteLine("newmtl shadow");
            materialWriter.WriteLine("Kd 0 0 0");
            materialWriter.WriteLine("newmtl shadowgradient");
            materialWriter.WriteLine("Kd 0.1 0.1 0.1");

            foreach (var materialName in materialNames)
            {
                materialWriter.WriteLine($"newmtl {materialName.Key}");
                if (materialName.Value == null)
                {
                    materialWriter.WriteLine("Kd 0 0 0");
                }
                else
                {
                    materialWriter.WriteLine($"map_Kd palette{materialName.Value}.bmp");
                }
            }
        }

        public void ReadFromOBJ(TextReader reader, Stream unknownData)
        {
            if (unknownData != null)
            {
                MenuFrontWheelRadius = unknownData.ReadUShort();
                MenuFrontWheelWidth = unknownData.ReadUShort();
                MenuRearWheelRadius = unknownData.ReadUShort();
                MenuRearWheelWidth = unknownData.ReadUShort();
                unknownData.ReadUShort(); // top 2b of LOD count, always 00 00
                unknownData.Read(UnknownAll);
                LOD0MaxDistance = UnknownAll.Skip(2).Take(2).ToArray().ReadUShort(); // dirty parsing of unknown data file in lieu of replacing it
                LOD1MaxDistance = UnknownAll.Skip(10).Take(2).ToArray().ReadUShort();
                LOD2MaxDistance = UnknownAll.Skip(18).Take(2).ToArray().ReadUShort();
            }

            var lods = new LOD[3];
            var wheelPositions = new WheelPosition[4];
            string line;
            int currentWheelPosition = -1;
            short currentWheelPositionWValue = 0;
            int currentLODNumber = -1;
            bool shadow = false;
            string currentMaterial = "untextured";
            LOD currentLOD = null;
            var vertices = new List<Vertex>();
            var normals = new List<Normal>();
            var uvCoords = new List<UVCoordinate>();
            var usedVertexIDs = new List<int>();
            var usedNormalIDs = new List<int>();
            int shadowVertexStartID = 0;
            double currentScale = 1;
            do
            {
                line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                {
                    continue;
                }

                try
                {
                    if (line.StartsWith("o ") || line.StartsWith("g "))
                    {
                        if (currentLOD != null)
                        {
                            currentLOD.ReadFromOBJ(vertices, normals, usedVertexIDs, usedNormalIDs);
                            usedVertexIDs.Clear();
                            usedNormalIDs.Clear();
                        }
                        currentScale = 1;
                        currentLOD = null;
                        currentLODNumber = -1;
                        shadow = false;
                        currentWheelPosition = -1;
                        currentWheelPositionWValue = 0;

                        string[] objectNameParts = line.Split(' ')[1].Split('/');
                        string objectName = objectNameParts[0];

                        if (objectName.StartsWith("lod"))
                        {
                            if (!int.TryParse(objectName.Replace("lod", ""), out currentLODNumber))
                            {
                                throw new Exception($"Could not read LOD number from object name '{objectName}'");
                            }
                            currentLOD = new LOD();
                            currentLOD.PrepareForOBJRead(unknownData);
                            lods[currentLODNumber] = currentLOD;
                            currentScale = GetScale(objectNameParts);
                            currentLOD.Scale = LOD.ConvertScale(currentScale);
                        }
                        else if (objectName.StartsWith("shadow"))
                        {
                            shadow = true;
                            Shadow = new Shadow();
                            Shadow.PrepareForOBJRead(unknownData);
                            shadowVertexStartID = vertices.Count;
                            currentScale = GetScale(objectNameParts);
                            Shadow.Scale = LOD.ConvertScale(currentScale);
                        }
                        else if (objectName.StartsWith("wheelpos"))
                        {
                            if (!int.TryParse(objectName.Replace("wheelpos", ""), out currentWheelPosition))
                            {
                                throw new Exception($"Could not read wheel position number from object name '{objectName}'");
                            }
                            foreach (string namePart in objectNameParts)
                            {
                                string[] keyAndValue = namePart.Split('=');
                                if (keyAndValue.Length == 2 && keyAndValue[0] == "w")
                                {
                                    currentWheelPositionWValue = short.Parse(keyAndValue[1]);
                                }
                            }
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

                            if (currentWheelPosition != -1)
                            {
                                var position = new WheelPosition();
                                position.ReadFromOBJ(vertex, currentWheelPositionWValue);
                                wheelPositions[currentWheelPosition] = position;
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
                        currentMaterial = line.Split(' ')[1];
                    }
                    else if (line.StartsWith("f "))
                    {
                        if (shadow)
                        {
                            var polygon = new ShadowPolygon();
                            polygon.ReadFromOBJ(line, Shadow.Vertices, shadowVertexStartID, currentMaterial);
                            if (polygon.IsQuad)
                            {
                                Shadow.Quads.Add(polygon);
                            }
                            else
                            {
                                Shadow.Triangles.Add(polygon);
                            }
                        }
                        else if (currentWheelPosition != -1)
                        {
                            continue;
                        }
                        else if (currentMaterial.StartsWith("untextured"))
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
                catch (Exception exception)
                {
                    throw new Exception($"Line: {line}\r\n{exception.Message}", exception);
                }
            }
            while (line != null);

            currentLOD?.ReadFromOBJ(vertices, normals, usedVertexIDs, usedNormalIDs);

            if (wheelPositions.Any(position => position == null))
            {
                throw new Exception("One or more of the four wheel position objects are missing.");
            }

            if (lods.Any(lod => lod == null))
            {
                throw new Exception("One or more of the three LOD objects are missing.");
            }

            if (Shadow == null)
            {
                throw new Exception("The shadow object is missing.");
            }

            Shadow.GenerateBoundingBox();
            LODs = lods.ToList();
            WheelPositions = wheelPositions.ToList();
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