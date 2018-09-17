using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class Vertex
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public ushort Padding { get; set; } = 0;

        public void ReadFromCDO(Stream stream)
        {
            X = stream.ReadShort();
            Y = stream.ReadShort();
            Z = stream.ReadShort();
            Padding = stream.ReadUShort();
        }
    }
}