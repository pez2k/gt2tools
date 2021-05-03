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

        public void ReadFromCAR(Stream stream)
        {
            ReadFromCDO(stream);
            Z = (short)(0 - Z);
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteShort(X);
            stream.WriteShort(Y);
            stream.WriteShort(Z);
            stream.WriteUShort(Padding);
        }

        public void WriteToOBJ(TextWriter writer) => writer.WriteLine($"v {(double)X / 10000} {(double)Y / 10000} {(double)Z / 10000}");
    }
}