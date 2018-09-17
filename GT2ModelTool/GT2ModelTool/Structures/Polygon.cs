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

            if (!values.Contains(Unknown12)) {
                values.Add(Unknown12);
            }

            //Vertex0Normal = normals[Unknown12];
            //Vertex1Normal = normals[Unknown6];
            //Vertex2Normal = normals[Unknown7];
            //Vertex3Normal = normals[Unknown8];
        }
    }
}