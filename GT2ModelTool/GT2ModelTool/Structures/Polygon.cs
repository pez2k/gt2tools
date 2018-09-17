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

        public virtual void ReadFromCDO(Stream stream, bool isQuad, List<Vertex> vertices)
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

            stream.Position += 0x0C;
        }
    }
}