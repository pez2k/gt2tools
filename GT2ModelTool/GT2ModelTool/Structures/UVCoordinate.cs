using System;
using System.IO;

namespace GT2.ModelTool.Structures
{
    public class UVCoordinate
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            X = (byte)stream.ReadByte();
            Y = (byte)stream.ReadByte();
        }

        public void ReadFromCAR(Stream stream)
        {
            X = (byte)stream.ReadByte();
            int y = stream.ReadByte();
            if (y >= 32)
            {
                y -= 32; // why? other data?
            }
            Y = (byte)y;
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteByte(X);
            stream.WriteByte(Y);
        }

        public void WriteToOBJ(TextWriter writer) => writer.WriteLine($"vt {X / 255M} {1 - (Y / 223M)}");

        public void ReadFromOBJ(string line)
        {
            string[] parts = line.Split(' ');
            if (parts.Length < 3 || parts.Length > 4)
            {
                throw new Exception("UV coord does not contain exactly two coordinate values");
            }
            decimal xValue = decimal.Parse(parts[1]);
            decimal yValue = decimal.Parse(parts[2]);
            if (xValue < 0 || xValue > 1 || yValue < 0 || yValue > 1)
            {
                throw new Exception("UV coords are outside of range 0 to 1");
            }

            X = (byte)Math.Round(xValue * 255);
            Y = (byte)Math.Round((1 - yValue) * 223);
        }
    }
}