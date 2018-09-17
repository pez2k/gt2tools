using System.IO;

namespace GT2.ModelTool.Structures
{
    public class WheelPosition
    {
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public ushort Z { get; set; }
        public ushort Scale { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            stream.Position += 0x08;
        }
    }
}