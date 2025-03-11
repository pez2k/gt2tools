using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class Model
    {
        public ushort Unknown1 { get; set; }
        public ushort Unknown2 { get; set; }
        public ushort Unknown3 { get; set; }
        public ushort Unknown4 { get; set; }
        public List<WheelPosition> WheelPositions { get; set; } = new List<WheelPosition>(4);
        public byte[] Unknown5 { get; set; } = new byte[26];
        public List<LOD> LODs { get; set; }
        public Shadow Shadow { get; set; }

        public void ReadFromCDO(Stream stream) {
            stream.Position = 0x08;
            Unknown1 = stream.ReadUShort();
            if (Unknown1 == 0) {
                stream.Position = 0x18;
                Unknown1 = stream.ReadUShort();
            }

            Unknown2 = stream.ReadUShort();
            Unknown3 = stream.ReadUShort();
            Unknown4 = stream.ReadUShort();

            for (int i = 0; i < 4; i++) {
                var wheelPosition = new WheelPosition();
                wheelPosition.ReadFromCDO(stream);
                WheelPositions.Add(wheelPosition);
            }

            stream.Position += 0x828;
            ushort lodCount = stream.ReadUShort();
            LODs = new List<LOD>(lodCount);

            stream.Read(Unknown5);

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

            Unknown1 = stream.ReadUShort();
            Unknown2 = stream.ReadUShort();
            Unknown3 = stream.ReadUShort();
            Unknown4 = stream.ReadUShort();

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
            stream.WriteUShort(Unknown1);
            stream.WriteUShort(Unknown2);
            stream.WriteUShort(Unknown3);
            stream.WriteUShort(Unknown4);

            foreach (WheelPosition wheelPosition in WheelPositions)
            {
                wheelPosition.WriteToCDO(stream);
            }

            stream.Position = 0x868;
            stream.WriteUShort((ushort)LODs.Count);
            stream.Write(Unknown5);

            foreach (LOD lod in LODs)
            {
                lod.WriteToCDO(stream);
            }

            Shadow.WriteToCDO(stream);
        }

        public void WriteToOBJ(TextWriter modelWriter, TextWriter materialWriter, string filename, Stream unknownData)
        {
            unknownData.WriteUShort(Unknown1);
            unknownData.WriteUShort(Unknown2);
            unknownData.WriteUShort(Unknown3);
            unknownData.WriteUShort(Unknown4);
            unknownData.Write(Unknown5);

            modelWriter.WriteLine($"mtllib {filename}.mtl");
            
            int vertexNumber = 1;
            int normalNumber = 1;
            int coordNumber = 1;
            var materialNames = new Dictionary<string, int?>();

            for (int i = 0; i < WheelPositions.Count; i++)
            {
                WheelPositions[i].WriteToOBJ(modelWriter, i, vertexNumber);
                vertexNumber++;
            }

            for (int i = 0; i < LODs.Count; i++)
            {
                LODs[i].WriteToOBJ(modelWriter, i, vertexNumber, normalNumber, coordNumber, materialNames, unknownData);
                vertexNumber += LODs[i].Vertices.Count;
                normalNumber += LODs[i].Normals.Count;
                coordNumber += LODs[i].GetAllUVCoords().Count;
            }

            Shadow.WriteToOBJ(modelWriter, vertexNumber, unknownData);
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
                Unknown1 = unknownData.ReadUShort();
                Unknown2 = unknownData.ReadUShort();
                Unknown3 = unknownData.ReadUShort();
                Unknown4 = unknownData.ReadUShort();
                unknownData.Read(Unknown5);
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