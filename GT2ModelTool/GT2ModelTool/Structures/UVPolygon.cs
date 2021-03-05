using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class UVPolygon : Polygon
    {
        public UVCoordinate Vertex0UV { get; set; } = new UVCoordinate();
        public ushort PaletteIndex { get; set; }
        public UVCoordinate Vertex1UV { get; set; } = new UVCoordinate();
        public byte Unknown13 { get; set; }
        public byte Unknown14 { get; set; }
        public UVCoordinate Vertex2UV { get; set; } = new UVCoordinate();
        public UVCoordinate Vertex3UV { get; set; } = new UVCoordinate();

        public override void ReadFromCDO(Stream stream, bool isQuad, List<Vertex> vertices, List<Normal> normals)
        {
            base.ReadFromCDO(stream, isQuad, vertices, normals);

            Vertex0UV.ReadFromCDO(stream);
            ushort rawPaletteIndex = stream.ReadUShort();
            PaletteIndex = (ushort)((rawPaletteIndex >> 4) + (rawPaletteIndex & 0x3F));
            Vertex1UV.ReadFromCDO(stream);
            Unknown13 = (byte)stream.ReadByte();
            Unknown14 = (byte)stream.ReadByte();

            if (Unknown13 != 0x00 || Unknown14 != 0x00)
            {
                throw new System.Exception("Last unknowns in Polygon not zero");
            }

            Vertex2UV.ReadFromCDO(stream);
            Vertex3UV.ReadFromCDO(stream);

            if (!isQuad && (Vertex3UV.X != 0x00 || Vertex3UV.Y != 0x00))
            {
                throw new System.Exception("Vertex 3 UVs in Triangle not zero");
            }
        }

        public override void ReadFromCAR(Stream stream, bool isQuad, List<Vertex> vertices, List<Normal> normals)
        {
            base.ReadFromCAR(stream, isQuad, vertices, normals);

            Vertex0UV.ReadFromCAR(stream);
            ushort rawPaletteIndex = stream.ReadUShort();
            PaletteIndex = (ushort)((rawPaletteIndex >> 4) + (rawPaletteIndex & 0x3F));
            Vertex1UV.ReadFromCAR(stream);
            Unknown13 = (byte)stream.ReadByte();
            Unknown14 = (byte)stream.ReadByte();

            if (Unknown13 != 0x00 || Unknown14 != 0x00)
            {
                throw new System.Exception("Last unknowns in Polygon not zero");
            }

            Vertex2UV.ReadFromCAR(stream);
            Vertex3UV.ReadFromCAR(stream);

            if (!isQuad && (Vertex3UV.X != 0x00 || Vertex3UV.Y != 0x00))
            {
                throw new System.Exception("Vertex 3 UVs in Triangle not zero");
            }
        }

        public override void WriteToCDO(Stream stream, bool isQuad, List<Vertex> vertices, List<Normal> normals)
        {
            base.WriteToCDO(stream, isQuad, vertices, normals);

            Vertex0UV.WriteToCDO(stream);
            stream.WriteUShort((ushort)(((PaletteIndex & 0x0C) << 4) + (PaletteIndex & 0x03)));
            Vertex1UV.WriteToCDO(stream);
            stream.WriteByte(Unknown13);
            stream.WriteByte(Unknown14);
            Vertex2UV.WriteToCDO(stream);
            Vertex3UV.WriteToCDO(stream);
        }
    }
}