using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class WheelPosition
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public ushort Scale { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            X = stream.ReadShort(); // these are probably in the wrong order
            Y = stream.ReadShort();
            Z = stream.ReadShort();
            Scale = stream.ReadUShort();
        }

        public void ReadFromCAR(Stream stream)
        {
            ReadFromCDO(stream);
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteShort(X);
            stream.WriteShort(Y);
            stream.WriteShort(Z);
            stream.WriteUShort(Scale);
        }
    }
}