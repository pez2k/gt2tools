﻿using System.IO;

namespace GT2.ModelTool.Structures
{
    public class UVTriangle : Triangle
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
        public byte Unknown15 { get; set; }
        public byte Unknown16 { get; set; }

        public new void ReadFromCDO(Stream stream)
        {
            stream.Position += 0x1C;
        }
    }
}