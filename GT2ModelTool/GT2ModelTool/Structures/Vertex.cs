using System;
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

        public void ReadFromOBJ(string line)
        {
            string[] parts = line.Split(' ');
            if (parts.Length != 4)
            {
                throw new Exception($"Line: {line}\r\nVertex does not contain exactly three coordinate values.");
            }
            X = (short)Math.Round(double.Parse(parts[1]) * 10000);
            Y = (short)Math.Round(double.Parse(parts[2]) * 10000);
            Z = (short)Math.Round(double.Parse(parts[3]) * 10000);
        }
    }
}