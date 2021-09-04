using System;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class ShadowVertex
    {
        public short X { get; set; }
        public short Z { get; set; }

        public void ReadFromCDO(Stream stream)
        {
            X = stream.ReadShort();
            Z = stream.ReadShort();
        }

        public void ReadFromCAR(Stream stream)
        {
            X = stream.ReadShort();
            stream.ReadShort(); // unused Y, always 0
            Z = stream.ReadShort();
            Z = (short)(0 - Z); // is this needed like with the main model verts?
            stream.Position += 2; // padding
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteShort(X);
            stream.WriteShort(Z);
        }

        public void WriteToOBJ(TextWriter writer, double scale) =>
            writer.WriteLine($"v {X * scale * Vertex.UnitsToMetres} 0 {Z * scale * Vertex.UnitsToMetres}");

        public void ReadFromOBJ(string line, double scale)
        {
            string[] parts = line.Split(' ');
            if (parts.Length != 4)
            {
                throw new Exception("Shadow vertex does not contain exactly three coordinate values.");
            }
            X = (short)Math.Round(double.Parse(parts[1]) / scale / Vertex.UnitsToMetres);
            Z = (short)Math.Round(double.Parse(parts[3]) / scale / Vertex.UnitsToMetres);
        }
    }
}