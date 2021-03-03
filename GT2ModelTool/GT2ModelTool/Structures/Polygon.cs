using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using StreamExtensions;

namespace GT2.ModelTool.Structures
{
    public class Polygon
    {
        public Vertex Vertex0 { get; set; }
        public Vertex Vertex1 { get; set; }
        public Vertex Vertex2 { get; set; }
        public Vertex Vertex3 { get; set; }
        
        public int RenderOrder { get; set; }
        public int RenderFlags { get; set; }
        public int FaceType { get; set; }

        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }
        public byte Unknown4 { get; set; }
        public byte Unknown5 { get; set; }
        public byte Unknown6 { get; set; }
        public byte Unknown7 { get; set; }
        public byte Unknown8 { get; set; }
        public byte Unknown9 { get; set; }
        public byte Unknown10 { get; set; }
        public byte Unknown11 { get; set; }
        public byte Unknown12 { get; set; }
        public Normal Vertex0Normal { get; set; }
        public Normal Vertex1Normal { get; set; }
        public Normal Vertex2Normal { get; set; }
        public Normal Vertex3Normal { get; set; }

        public static List<byte> values = new List<byte>();

        public virtual void ReadFromCDO(Stream stream, bool isQuad, List<Vertex> vertices, List<Normal> normals)
        {
            byte vertex0Ref = (byte)stream.ReadByte();
            Vertex0 = vertices[vertex0Ref];
            byte vertex1Ref = (byte)stream.ReadByte();
            Vertex1 = vertices[vertex1Ref];
            byte vertex2Ref = (byte)stream.ReadByte();
            Vertex2 = vertices[vertex2Ref];
            byte vertex3Ref = (byte)stream.ReadByte();
            if (isQuad)
            {
                Vertex3 = vertices[vertex3Ref];
            }
            else if (vertex3Ref != 0x00) {
                throw new Exception("Vertex 3 in Triangle not zero");
            }

            ushort a = stream.ReadUShort();
            ushort b = stream.ReadUShort();
            int c = stream.ReadInt();
            int d = stream.ReadInt();

            RenderOrder = a & 0x1F;
            if (RenderOrder == 0b1000)
            {
                Debug.WriteLine($"Render order of 0b1000 found at {stream.Position}");
            }
            int normal0Ref = (a >> 5) & 0x1FF;
            Vertex0Normal = normals[normal0Ref];

            RenderFlags = b >> 12;
            if (RenderFlags != 0b1100 && RenderFlags != 0b1000 && RenderFlags != 0)
            {
                Debug.WriteLine($"Render flags of {RenderFlags} found at {stream.Position}");
            }

            int normal1Ref = (c >> 1) & 0x1FF;
            Vertex1Normal = normals[normal1Ref];
            int normal2Ref = (c >> 10) & 0x1FF;
            Vertex2Normal = normals[normal2Ref];
            int normal3Ref = (c >> 20) & 0x1FF;
            Vertex3Normal = normals[normal3Ref];

            FaceType = d >> 24;
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
            if (stream.Position >= 3848)
            {
            }

            byte vertexByte0 = (byte)stream.ReadByte();
            byte vertexByte1 = (byte)stream.ReadByte();
            byte vertexByte2 = (byte)stream.ReadByte();
            byte vertexByte3 = (byte)stream.ReadByte();
            byte vertexByte4 = (byte)stream.ReadByte();
            byte vertexByte5 = (byte)stream.ReadByte();

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

            Unknown3 = (byte)stream.ReadByte(); // many values - bottom bit tex only, top 5 tex only
            Unknown4 = (byte)stream.ReadByte(); // 0, 1, 2, 3, 4, 5, 6, 128, 133 - top bit set on both unt and tex, not on tex quads but two tex tris
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
            Unknown5 = (byte)stream.ReadByte(); // many values - very top bit set for any sort of tex poly, so normal 2 at most - all bits set at least once
                                                // all bits set for tex only

            Unknown6 = (byte)stream.ReadByte(); // many values - all 8 bits are only set for tex quads, so part of normal 3?
            Unknown7 = (byte)stream.ReadByte(); // 0, 1, 2, 3 - top bit of final normal index? - only set for tex quads, so top of normal 3
            Unknown8 = (byte)stream.ReadByte(); // always 0
            //         8         7         6         5         4         3       vb5
            // 0000 0000 0000 0xxx xxxx xxxx xxxx xxxx x000 xxxx xxxx x0xx ---- ---!
            //                 ttt tttt tttt tttt tttt a    tttt tttt t tt tttt ttt
            //                 qqq qqqq qqqq
            //                 333 3333 33?2 2222 2222 ?    1111 1111 1 00 0000 000!

            /*if ((vertexByte5 & 0b0000_0001) != 0)
            {
                Debug.WriteLine($"{GetType()} -- {isQuad}");
            }*/

            int normal0Maybe = vertexByte5 + (Unknown3 * 256);
            normal0Maybe >>= 1;
            normal0Maybe &= 0x1FF;
            Vertex0Normal = normals[normal0Maybe];

            int normal1Maybe = Unknown3 + (Unknown4 * 256);
            normal1Maybe >>= 3;
            normal1Maybe &= 0x1FF;
            Vertex1Normal = normals[normal1Maybe];

            int normal2Maybe = Unknown5 + (Unknown6 * 256);
            normal2Maybe &= 0x1FF;
            Vertex2Normal = normals[normal2Maybe];

            int normal3Maybe = Unknown6 + (Unknown7 * 256);
            normal3Maybe >>= 2;
            normal3Maybe &= 0x1FF;
            Vertex3Normal = normals[normal3Maybe];

            Unknown9 = (byte)stream.ReadByte(); // 00 for untextured, FF for textured
            Unknown10 = (byte)stream.ReadByte(); // 00 for untextured, FF for textured
            Unknown11 = (byte)stream.ReadByte(); // 00 for untextured, FF for textured

            Unknown12 = (byte)stream.ReadByte(); // 21 for unt tri, 29 unt quad, 25 tex tri, 2D tex quad
            // 100001 / 21 unt tri - GT2 + 1
            // 101001 / 29 unt quad - GT2 + 1
            // 100101 / 25 tex tri
            // 101101 / 2D tex quad
            if (Unknown12 == 33 || Unknown12 == 41)
            {
                FaceType = Unknown12 - 1;
            }
            else
            {
                FaceType = Unknown12;
            }
        }

        public virtual void WriteToCDO(Stream stream, bool isQuad, List<Vertex> vertices)
        {
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

            stream.WriteByte(Unknown1);
            stream.WriteByte(Unknown2);
            stream.WriteByte(Unknown3);
            stream.WriteByte(Unknown4);
            stream.WriteByte(Unknown5);
            stream.WriteByte(Unknown6);
            stream.WriteByte(Unknown7);
            stream.WriteByte(Unknown8);
            stream.WriteByte(Unknown9);
            stream.WriteByte(Unknown10);
            stream.WriteByte(Unknown11);
            stream.WriteByte(Unknown12);
        }
    }
}