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
        public ushort FrontWheelRadius { get; set; }
        public ushort FrontWheelWidth { get; set; }
        public ushort RearWheelRadius { get; set; }
        public ushort RearWheelWidth { get; set; }
        public List<WheelPosition> WheelPositions { get; set; } = new List<WheelPosition>(4);
        public byte[] UnknownAll { get; set; } = new byte[26];
        public ushort LOD0Padding { get; set; }
        public ushort LOD0MaxDistance { get; set; }
        public uint LOD0Unknown { get; set; }
        public ushort LOD1Padding { get; set; }
        public ushort LOD1MaxDistance { get; set; }
        public uint LOD1Unknown { get; set; } // non-zero
        public ushort LOD2Padding { get; set; }
        public ushort LOD2MaxDistance { get; set; }
        public uint LOD2Unknown { get; set; } // non-zero

        public List<LOD> LODs { get; set; }
        public Shadow Shadow { get; set; }

        public void ReadFromCDO(Stream stream) {
            stream.Position = 0x08;
            FrontWheelRadius = stream.ReadUShort();
            if (FrontWheelRadius == 0) {
                stream.Position = 0x18;
                FrontWheelRadius = stream.ReadUShort();
            }

            FrontWheelWidth = stream.ReadUShort();
            RearWheelRadius = stream.ReadUShort();
            RearWheelWidth = stream.ReadUShort();

            for (int i = 0; i < 4; i++) {
                var wheelPosition = new WheelPosition();
                wheelPosition.ReadFromCDO(stream);
                WheelPositions.Add(wheelPosition);
            }

            stream.Position += 0x828;
            ushort lodCount = stream.ReadUShort();
            LODs = new List<LOD>(lodCount);

            stream.Read(UnknownAll);
            stream.Position -= UnknownAll.Length;

            stream.Position += 2;
            LOD0Padding = stream.ReadUShort();
            LOD0MaxDistance = stream.ReadUShort();
            LOD0Unknown = stream.ReadUInt();
            LOD1Padding = stream.ReadUShort();
            LOD1MaxDistance = stream.ReadUShort();
            LOD1Unknown = stream.ReadUInt();
            LOD2Padding = stream.ReadUShort();
            LOD2MaxDistance = stream.ReadUShort();
            LOD2Unknown = stream.ReadUInt();

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

            FrontWheelRadius = stream.ReadUShort();
            FrontWheelWidth = stream.ReadUShort();
            RearWheelRadius = stream.ReadUShort();
            RearWheelWidth = stream.ReadUShort();

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
            // GT header
            stream.Write([0x47, 0x54, 0x02]);
            stream.Position = 0x18;
            stream.WriteUShort(FrontWheelRadius);
            stream.WriteUShort(FrontWheelWidth);
            stream.WriteUShort(RearWheelRadius);
            stream.WriteUShort(RearWheelWidth);

            foreach (WheelPosition wheelPosition in WheelPositions)
            {
                wheelPosition.WriteToCDO(stream);
            }

            stream.Position = 0x868;
            stream.WriteUShort((ushort)LODs.Count);
            stream.Write(UnknownAll);

            foreach (LOD lod in LODs)
            {
                lod.WriteToCDO(stream);
            }

            Shadow.WriteToCDO(stream);
        }

        public void WriteToOBJ(TextWriter modelWriter, TextWriter materialWriter, string filename, Stream unknownData, ModelMetadata metadata)
        {
            unknownData.WriteUShort(FrontWheelRadius);
            unknownData.WriteUShort(FrontWheelWidth);
            unknownData.WriteUShort(RearWheelRadius);
            unknownData.WriteUShort(RearWheelWidth);
            unknownData.Write(UnknownAll);

            metadata.FrontWheelRadius = FrontWheelRadius;
            metadata.FrontWheelWidth = FrontWheelWidth;
            metadata.RearWheelRadius = RearWheelRadius;
            metadata.RearWheelWidth = RearWheelWidth;
            metadata.LOD0.HeaderAlwaysZero = LOD0Padding;
            metadata.LOD0.MaxDistance = LOD0MaxDistance;
            metadata.LOD0.HeaderUnknown = LOD0Unknown;
            metadata.LOD1.HeaderAlwaysZero = LOD1Padding;
            metadata.LOD1.MaxDistance = LOD1MaxDistance;
            metadata.LOD1.HeaderUnknown = LOD1Unknown;
            metadata.LOD2.HeaderAlwaysZero = LOD2Padding;
            metadata.LOD2.MaxDistance = LOD2MaxDistance;
            metadata.LOD2.HeaderUnknown = LOD2Unknown;

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
                FrontWheelRadius = unknownData.ReadUShort();
                FrontWheelWidth = unknownData.ReadUShort();
                RearWheelRadius = unknownData.ReadUShort();
                RearWheelWidth = unknownData.ReadUShort();
                unknownData.Read(UnknownAll);
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