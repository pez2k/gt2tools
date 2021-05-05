using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class WheelPosition
    {
        public short X { get; set; }
        public short Y { get; set; } // vertical
        public short Z { get; set; } // forwards / backwards
        public short Scale { get; set; } // track width? probably signed?

        public void ReadFromCDO(Stream stream)
        {
            X = stream.ReadShort(); // these are probably in the wrong order
            Y = stream.ReadShort();
            Z = stream.ReadShort();
            Scale = stream.ReadShort();
        }

        public void ReadFromCAR(Stream stream)
        {
            ReadFromCDO(stream);
            Scale = X; // zero in GT1, using the X value isn't totally correct but better than nothing
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteShort(X);
            stream.WriteShort(Y);
            stream.WriteShort(Z);
            stream.WriteShort(Scale);
        }

        public void WriteToOBJ(TextWriter writer, int wheelNumber)
        {
            writer.WriteLine($"# scale: {Scale}");
            writer.WriteLine($"g wheelpos{wheelNumber}");
            writer.WriteLine($"v {(double)X / 10000} {(double)Y / 10000} {(double)Z / 10000}");
            wheelNumber++;
            writer.WriteLine($"f {wheelNumber} {wheelNumber} {wheelNumber}");
        }
    }
}