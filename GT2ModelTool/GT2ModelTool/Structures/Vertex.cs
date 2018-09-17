using System.IO;

namespace GT2.ModelTool.Structures
{
    public class Vertex
    {
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public ushort Z { get; set; }
        public ushort Padding { get; set; } = 0;

        public void ReadFromCDO(Stream stream)
        {
            stream.Position += 0x08;
        }
    }
}