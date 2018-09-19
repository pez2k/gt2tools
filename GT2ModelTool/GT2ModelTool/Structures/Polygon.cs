using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    public class Polygon
    {
        public Vertex Vertex0 { get; set; }
        public Vertex Vertex1 { get; set; }
        public Vertex Vertex2 { get; set; }
        public Vertex Vertex3 { get; set; }
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
                throw new System.Exception("Vertex 3 in Triangle not zero");
            }

            Unknown1 = (byte)stream.ReadByte(); // 241 max, 22 values
            Unknown2 = (byte)stream.ReadByte(); // 0 to 19 inclusive & complete in bistn
            Unknown3 = (byte)stream.ReadByte(); // always 0
            Unknown4 = (byte)stream.ReadByte(); // 0, 64, 128 or 192 - two bit flags?
            Unknown5 = (byte)stream.ReadByte(); // 254 max - 114 values, all multiples of 2, not quite a full list
            Unknown6 = (byte)stream.ReadByte(); // 253 max - 77 values, irregular
            Unknown7 = (byte)stream.ReadByte(); // 249 max - 60 values, irregular
            Unknown8 = (byte)stream.ReadByte(); // 0 in tris - 0, 1, 2, 3, 4, 5 values in bistn - three bit flags?
            Unknown9 = (byte)stream.ReadByte(); // always 0
            Unknown10 = (byte)stream.ReadByte(); // always 0
            Unknown11 = (byte)stream.ReadByte(); // always 0
            Unknown12 = (byte)stream.ReadByte(); // 32, 37, 40, 45
            // zero out all normals in savestate, then zero one by one until it affects a given poly - then find that poly and work out the relationship

            if (!values.Contains(Unknown12)) {
                values.Add(Unknown12);
            }

            // example numbers
            // tri
            // 144 -> 1001 0000
            //   9 -> 0000 1001
            //   0 -> 0000 0000
            // 128 -> 1000 0000
            // 154 -> 1001 1010
            //  56 -> 0011 1000
            //   1 -> 0000 0001
            //   0 -> 0000 0000
            //   0 -> 0000 0000
            //   0 -> 0000 0000
            //   0 -> 0000 0000
            //  37 -> 0010 0101

            // 1001 0000 0000 1001 0000 0000 1000 0000 1001 1010 0011 1000 0000 0001 0000 0000 0000 0000 0000 0000 0000 0000 0010 0101

            // quad
            //  48 -> 0011 0000
            //  18 -> 0001 0010
            //   0 -> 0000 0000
            // 128 -> 1000 0000
            //  36 -> 0010 0100
            //  77 -> 0100 1101
            // 162 -> 1010 0010
            //   4 -> 0000 0100
            //   0 -> 0000 0000
            //   0 -> 0000 0000
            //   0 -> 0000 0000
            //  45 -> 0010 1101

            // 0011 0000 0001 0010 0000 0000 1000 0000 0010 0100 0100 1101 1010 0010 0000 0100 0000 0000 0000 0000 0000 0000 0010 1101

            // 1010 0011 - max normal ID
            // T 1001000000001001000000001000 0000100 1101000 1110000 0000001 0000000 000000000000000000000000000100101
            // Q 0011000000010010000000001000 0000001 0010001 0011011 0100010 0000010 000000000000000000000000000101101
            //                                 4 / 1  104/17  112/27  1 / 66  0 / 2

            //Vertex0Normal = normals[Unknown12];
            //Vertex1Normal = normals[Unknown6];
            //Vertex2Normal = normals[Unknown7];
            //Vertex3Normal = normals[Unknown8];
        }

        public virtual void ReadFromCAR(Stream stream, bool isQuad, List<Vertex> vertices, List<Normal> normals)
        {
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

            //Unknown1 = (byte)stream.ReadByte();
            //Unknown2 = (byte)stream.ReadByte();
            Unknown3 = (byte)stream.ReadByte();
            Unknown4 = (byte)stream.ReadByte();
            Unknown5 = (byte)stream.ReadByte();
            Unknown6 = (byte)stream.ReadByte();
            Unknown7 = (byte)stream.ReadByte();
            Unknown8 = (byte)stream.ReadByte();
            Unknown9 = (byte)stream.ReadByte();
            Unknown10 = (byte)stream.ReadByte();
            Unknown11 = (byte)stream.ReadByte();
            Unknown12 = (byte)stream.ReadByte();
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