using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    public class UVPolygon : Polygon
    {
        public byte Vertex0UVX { get; set; }
        public byte Vertex0UVY { get; set; }
        public ushort PaletteIndex { get; set; }
        public byte Vertex1UVX { get; set; }
        public byte Vertex1UVY { get; set; }
        public byte Unknown13 { get; set; }
        public byte Unknown14 { get; set; }
        public byte Vertex2UVX { get; set; }
        public byte Vertex2UVY { get; set; }
        public byte Vertex3UVX { get; set; }
        public byte Vertex3UVY { get; set; }

        public override void ReadFromCDO(Stream stream, bool isQuad, List<Vertex> vertices)
        {
            base.ReadFromCDO(stream, isQuad, vertices);

            Vertex0UVX = (byte)stream.ReadByte();
            Vertex0UVY = (byte)stream.ReadByte();
            stream.Position += sizeof(ushort);
            Vertex1UVX = (byte)stream.ReadByte();
            Vertex1UVY = (byte)stream.ReadByte();
            Unknown13 = (byte)stream.ReadByte();
            Unknown14 = (byte)stream.ReadByte();

            if (Unknown13 != 0x00 || Unknown14 != 0x00)
            {
                throw new System.Exception("Last unknowns in Polygon not zero");
            }

            Vertex2UVX = (byte)stream.ReadByte();
            Vertex2UVY = (byte)stream.ReadByte();
            Vertex3UVX = (byte)stream.ReadByte();
            Vertex3UVY = (byte)stream.ReadByte();

            if (!isQuad && (Vertex3UVX != 0x00 || Vertex3UVY != 0x00))
            {
                throw new System.Exception("Vertex 3 UVs in Triangle not zero");
            }
        }
    }
}