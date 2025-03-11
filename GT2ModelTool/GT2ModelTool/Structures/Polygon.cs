using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using StreamExtensions;

namespace GT2.ModelTool.Structures
{
    using ExportMetadata;

    public class Polygon
    {
        public Vertex Vertex0 { get; set; }
        public Vertex Vertex1 { get; set; }
        public Vertex Vertex2 { get; set; }
        public Vertex Vertex3 { get; set; }
        public int RenderOrder { get; set; } = 0b10000;
        public int RenderFlags { get; set; }
        public int FaceType { get; set; }
        public Normal Vertex0Normal { get; set; }
        public Normal Vertex1Normal { get; set; }
        public Normal Vertex2Normal { get; set; }
        public Normal Vertex3Normal { get; set; }
        public bool IsQuad => Vertex3 != null;

        public virtual void ReadFromCDO(Stream stream, bool isQuad, List<Vertex> vertices, List<Normal> normals)
        {
            byte vertex0Ref = stream.ReadSingleByte();
            Vertex0 = vertices[vertex0Ref];
            byte vertex1Ref = stream.ReadSingleByte();
            Vertex1 = vertices[vertex1Ref];
            byte vertex2Ref = stream.ReadSingleByte();
            Vertex2 = vertices[vertex2Ref];
            byte vertex3Ref = stream.ReadSingleByte();
            if (isQuad)
            {
                Vertex3 = vertices[vertex3Ref];
            }
            else if (vertex3Ref != 0x00)
            {
                throw new Exception("Vertex 3 in Triangle not zero");
            }

            ushort renderOrderAndFirstNormalData = stream.ReadUShort();
            ushort renderFlagsData = stream.ReadUShort();
            int normalsData = stream.ReadInt();
            int faceTypeData = stream.ReadInt();

            RenderOrder = renderOrderAndFirstNormalData & 0x1F;
            if (RenderOrder == 0b1000)
            {
                Debug.WriteLine($"Render order of 0b1000 found at {stream.Position}");
            }

            if (normals.Any())
            {
                int normal0Ref = (renderOrderAndFirstNormalData >> 5) & 0x1FF;
                Vertex0Normal = normals[normal0Ref];
            }

            RenderFlags = renderFlagsData >> 12;
            if (RenderFlags != 0b1100 && RenderFlags != 0b1000 && RenderFlags != 0)
            {
                Debug.WriteLine($"Render flags of {RenderFlags} found at {stream.Position}");
            }

            if (normals.Any())
            {
                int normal1Ref = (normalsData >> 1) & 0x1FF;
                Vertex1Normal = normals[normal1Ref];
                int normal2Ref = (normalsData >> 10) & 0x1FF;
                Vertex2Normal = normals[normal2Ref];
                int normal3Ref = (normalsData >> 19) & 0x1FF;
                Vertex3Normal = normals[normal3Ref];
            }

            FaceType = faceTypeData >> 24;
            // 100000 (32) for untextured tri
            // 100101 (37) for textured tri
            // 101000 (40) for untextured quad
            // 101101 (45) for textured quad
            if (FaceType != 32 && FaceType != 37 && FaceType != 40 && FaceType != 45)
            {
                throw new Exception($"Unrecognised face type {FaceType}");
            }
        }

        public virtual void ReadFromCAR(Stream stream, bool isQuad, List<Vertex> vertices, List<Normal> normals)
        {
            byte vertexByte0 = stream.ReadSingleByte();
            byte vertexByte1 = stream.ReadSingleByte();
            byte vertexByte2 = stream.ReadSingleByte();
            byte vertexByte3 = stream.ReadSingleByte();
            byte vertexByte4 = stream.ReadSingleByte();
            byte vertexByte5 = stream.ReadSingleByte();

            // example bytes:
            //  0  1  2  3  4  5
            // 1E 3A 68 88 00 00 -> 30 29 26 0
            // 52 2E 60 88 00 00
            // 16 32 60 88 17 00
            // 1A 3A 70 80 1B 00
            // 22 AC 64 88 16 00
            // 41 DA BC 89 43 00

            /*
            
            1E - 0001 1110
            3A - 0011 1010
            68 - 0110 1000
            88 - 1000 1000
            
            0001 1110 0011 1010 0110 1000 1000 1000 0000 0000 0000 0000

            0001 1110
            0001 1101
            0001 1010

            0001 1110   - 30
            (0)001 1101 - 29
            (0)001 1010 - 26
            (0)001 0001 - 17 dec??
            (0)000 0000 - 0
            0000 0000 0000

             V      V      V      V
            0001111000111010011010001000100000000000 0000 0000

            49 - 0100 1001
            DA - 1101 1010
            BC - 1011 1100
            89 - 1000 1001
            43 - 0100 0011

            0100 1001 1101 1010 1011 1100 1000 1001 0100 0011 0000 0000

            0 1001001 1101101 0101111 0010001 0010100 0011000 00000

            0100 1001 - 73
            0110 1101 - 109
            0010 1111 - 47
            0001 0001 - 17 again
            0001 0100 - 20
            0001 1000 - 24
             
            */

            int vertex0Ref = ((vertexByte1 & 1) * 256) + vertexByte0;
            Vertex0 = vertices[vertex0Ref];
            int vertex1Ref = ((vertexByte2 & 2) * 128) + ((vertexByte2 & 1) * 128) + (vertexByte1 >> 1);
            Vertex1 = vertices[vertex1Ref];
            int vertex2Ref = ((vertexByte3 & 4) * 64) + ((vertexByte3 & 2) * 64) + ((vertexByte3 & 1) * 64) + (vertexByte2 >> 2);
            Vertex2 = vertices[vertex2Ref];
            int vertex3Ref = ((vertexByte5 & 1) * 256) + vertexByte4;
            if (isQuad)
            {
                Vertex3 = vertices[vertex3Ref];
            }
            /*else if (vertex3Ref != 0x00)
            {
                throw new System.Exception("Vertex 3 in Triangle not zero");
            }*/

            byte normalsByte1 = stream.ReadSingleByte(); // many values - bottom bit tex only, top 5 tex only
            byte normalsByte2 = stream.ReadSingleByte(); // 0, 1, 2, 3, 4, 5, 6, 128, 133 - top bit set on both unt and tex, not on tex quads but two tex tris
                                                     // bottom 3 bits set on tex only
                                                     // 0000 0000 - 0
                                                     // 0000 0001 - 1
                                                     // 0000 0010 - 2
                                                     // 0000 0011 - 3
                                                     // 0000 0100 - 4
                                                     // 0000 0101 - 5
                                                     // 0000 0110 - 6
                                                     // 1000 0000 - 128 + 0 - bottom bit of next normal index?
                                                     // 1000 0101 - 128 + 5
            byte normalsByte3 = stream.ReadSingleByte(); // many values - very top bit set for any sort of tex poly, so normal 2 at most - all bits set at least once
                                                     // all bits set for tex only

            byte normalsByte4 = stream.ReadSingleByte(); // many values - all 8 bits are only set for tex quads, so part of normal 3? - bit 2 never set?
            byte normalsByte5 = stream.ReadSingleByte(); // 0, 1, 2, 3 - top bit of final normal index? - only set for tex quads, so top of normal 3
            byte normalsByte6 = stream.ReadSingleByte(); // always 0
            //         8         7         6         5         4         3       vb5
            // 0000 0000 0000 0xxx xxxx xx0x xxxx xxxx x000 xxxx xxxx x0xx ---- ---v
            //                 ttt tttt tt t tttt tttt a    tttt tttt t tt tttt ttt
            //                 qqq qqqq qq q
            //                 333 3333 33 2 2222 2222 ?    1111 1111 1 00 0000 000v

            /*if ((vertexByte5 & 0b0000_0001) != 0)
            {
                Debug.WriteLine($"{GetType()} -- {isQuad}");
            }*/

            if ((normalsByte2 & 0b1000_0000) != 0)
            {
                // faces with this bit set seem to be side windows, wheelarch interiors, wing mounts
                RenderOrder = 0b10001;
            }

            int normal0Maybe = vertexByte5 + (normalsByte1 * 256);
            normal0Maybe >>= 1;
            normal0Maybe &= 0x1FF;
            Vertex0Normal = normals[normal0Maybe];

            int normal1Maybe = normalsByte1 + (normalsByte2 * 256);
            normal1Maybe >>= 3;
            normal1Maybe &= 0x1FF;
            Vertex1Normal = normals[normal1Maybe];

            int normal2Maybe = normalsByte3 + (normalsByte4 * 256);
            normal2Maybe &= 0x1FF;
            Vertex2Normal = normals[normal2Maybe];

            int normal3Maybe = normalsByte4 + (normalsByte5 * 256);
            normal3Maybe >>= 2;
            normal3Maybe &= 0x1FF;
            Vertex3Normal = normals[normal3Maybe];

            byte isTextured1 = stream.ReadSingleByte(); // 00 for untextured, FF for textured
            byte isTextured2 = stream.ReadSingleByte(); // 00 for untextured, FF for textured
            byte isTextured3 = stream.ReadSingleByte(); // 00 for untextured, FF for textured

            byte faceTypeData = stream.ReadSingleByte(); // 21 for unt tri, 29 unt quad, 25 tex tri, 2D tex quad
            // 100001 / 21 unt tri - GT2 + 1
            // 101001 / 29 unt quad - GT2 + 1
            // 100101 / 25 tex tri
            // 101101 / 2D tex quad
            if (faceTypeData == 33 || faceTypeData == 41)
            {
                FaceType = faceTypeData - 1;
            }
            else
            {
                FaceType = faceTypeData;
            }
        }

        public virtual void WriteToCDO(Stream stream, bool isQuad, List<Vertex> vertices, List<Normal> normals)
        {
            // break selected faces to make it obvious which ones they are in GT2CarViewer
            /*if (RenderOrder != 0b10000)
            {
                Vertex1 = Vertex0;
                Vertex2 = Vertex0;
            }*/

            stream.WriteByte((byte)vertices.IndexOf(Vertex0));
            stream.WriteByte((byte)vertices.IndexOf(Vertex1));
            stream.WriteByte((byte)vertices.IndexOf(Vertex2));
            if (isQuad)
            {
                stream.WriteByte((byte)vertices.IndexOf(Vertex3));
            }
            else
            {
                stream.WriteByte(0x00);
            }

            int normal0Ref = normals.IndexOf(Vertex0Normal);
            int normal1Ref = normals.IndexOf(Vertex1Normal);
            int normal2Ref = normals.IndexOf(Vertex2Normal);
            int normal3Ref = Vertex3Normal == null ? 0 : normals.IndexOf(Vertex3Normal);
            stream.WriteUShort((ushort)((normal0Ref << 5) + RenderOrder));
            stream.WriteUShort((ushort)(RenderFlags << 12));
            stream.WriteUInt((uint)((normal1Ref << 1) + (normal2Ref << 10) + (normal3Ref << 19)));
            stream.WriteUInt((uint)(FaceType << 24));
        }

        public void WriteToOBJ(TextWriter writer, bool isQuad, List<Vertex> vertices, List<Normal> normals,
                               int firstVertexNumber, int firstNormalNumber, Dictionary<string, int?> materialNames, List<MaterialMetadata> metadata)
        {
            string materialName = $"untextured/order={RenderOrder}/flags={RenderFlags}";
            materialNames[materialName] = null;
            metadata.Add(GenerateMaterialMetadata(materialName) with { IsUntextured = true });
            writer.WriteLine($"usemtl {materialName}");
            writer.WriteLine($"f {WriteVertexToOBJ(Vertex0, Vertex0Normal, vertices, normals, firstVertexNumber, firstNormalNumber)} " +
                             $"{WriteVertexToOBJ(Vertex1, Vertex1Normal, vertices, normals, firstVertexNumber, firstNormalNumber)} " +
                             $"{WriteVertexToOBJ(Vertex2, Vertex2Normal, vertices, normals, firstVertexNumber, firstNormalNumber)}" +
                             (isQuad ? $" {WriteVertexToOBJ(Vertex3, Vertex3Normal, vertices, normals, firstVertexNumber, firstNormalNumber)}" : ""));
        }

        protected MaterialMetadata GenerateMaterialMetadata(string materialName) =>
            new MaterialMetadata
            {
                Name = materialName,
                RenderOrder = RenderOrder,
                IsBrakeLight = (RenderFlags & 4) != 0,
                IsMatte = (RenderFlags & 8) == 0
            };

        private string WriteVertexToOBJ(Vertex vertex, Normal normal, List<Vertex> vertices, List<Normal> normals, int firstVertexNumber, int firstNormalNumber) =>
            $"{vertices.IndexOf(vertex) + firstVertexNumber}//{normals.IndexOf(normal) + firstNormalNumber}";

        public void ReadFromOBJ(string line, List<Vertex> vertices, List<Normal> normals, string currentMaterial, List<int> usedVertexIDs, List<int> usedNormalIDs)
        {
            string[] parts = line.Split(' ');
            if (parts.Length < 4 || parts.Length > 5)
            {
                throw new Exception("Face does not contain exactly three or four vertices.");
            }
            ParseMaterial(currentMaterial.Split('/'));
            (Vertex0, Vertex0Normal) = ParseVertex(parts[1], vertices, normals, usedVertexIDs, usedNormalIDs);
            (Vertex1, Vertex1Normal) = ParseVertex(parts[2], vertices, normals, usedVertexIDs, usedNormalIDs);
            (Vertex2, Vertex2Normal) = ParseVertex(parts[3], vertices, normals, usedVertexIDs, usedNormalIDs);
            if (parts.Length == 5)
            {
                (Vertex3, Vertex3Normal) = ParseVertex(parts[4], vertices, normals, usedVertexIDs, usedNormalIDs);
            }
            FaceType = IsQuad ? 40 : 32;
        }

        protected virtual void ParseMaterial(string[] parts)
        {
            foreach (string part in parts)
            {
                string[] pair = part.Split('=');
                if (pair[0] == "order")
                {
                    RenderOrder = int.Parse(pair[1]);
                }
                else if (pair[0] == "flags")
                {
                    RenderFlags = int.Parse(pair[1]);
                }
            }
        }

        private (Vertex v, Normal n) ParseVertex(string value, List<Vertex> vertices, List<Normal> normals, List<int> usedVertexIDs, List<int> usedNormalIDs)
        {
            string[] vertexData = value.Split('/');
            int vertexID = int.Parse(vertexData[0]) - 1;
            Vertex vertex = vertices[vertexID];
            usedVertexIDs.Add(vertexID);
            Normal normal = null;
            if (vertexData.Length > 2 && vertexData[2] != "")
            {
                int normalID = int.Parse(vertexData[2]) - 1;
                normal = normals[normalID];
                usedNormalIDs.Add(normalID);
            }
            return (vertex, normal);
        }
    }
}