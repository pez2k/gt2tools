﻿using System;
using System.IO;
using System.Linq;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class Vertex
    {
        public static double UnitsToMetres => 1 / 4096D; // from commongear's research

        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public short W { get; set; } = 0;

        public void ReadFromCDO(Stream stream)
        {
            X = stream.ReadShort();
            Y = stream.ReadShort();
            Z = stream.ReadShort();
            W = stream.ReadShort();
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
            stream.WriteShort(W);
        }

        public void WriteToOBJ(TextWriter writer, double scale) =>
            writer.WriteLine($"v {X * scale * UnitsToMetres} {Y * scale * UnitsToMetres} {Z * scale * UnitsToMetres}");

        public void ReadFromOBJ(string line, double scale)
        {
            string[] parts = line.Split(' ').Where(part => !string.IsNullOrWhiteSpace(part)).ToArray();
            if (parts.Length < 4 || parts.Length > 5)
            {
                throw new Exception("Vertex does not contain exactly three coordinate values");
            }
            X = (short)Math.Round(double.Parse(parts[1]) / scale / UnitsToMetres);
            Y = (short)Math.Round(double.Parse(parts[2]) / scale / UnitsToMetres);
            Z = (short)Math.Round(double.Parse(parts[3]) / scale / UnitsToMetres);
            if (parts.Length == 5)
            {
                W = (short)Math.Round(double.Parse(parts[4]) / scale / UnitsToMetres);
            }
        }

        public Vertex CreateDuplicate() => new Vertex { X = X, Y = Y, Z = Z, W = W };
    }
}