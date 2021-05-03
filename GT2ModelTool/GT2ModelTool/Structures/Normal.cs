using System;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class Normal
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public static double maxX;
        public static double maxY;
        public static double maxZ;
        public static double minX;
        public static double minY;
        public static double minZ;

        public void ReadFromCDO(Stream stream)
        {
            // 0110 1000 1000 1000 1010 0011 1010 1100
            // Z 01 1010 0010
            // Y 00 1000 1010
            // X 00 1110 1011
            // p 00
            double scale = 500; // From commongear's research
            uint i = stream.ReadUInt();
            X = ShiftSignedBits(i, 2) / scale;
            Y = ShiftSignedBits(i, 12) / scale;
            Z = ShiftSignedBits(i, 22) / scale;
            ValidateUnitVector();

            if (X > maxX) { maxX = X; }
            if (Y > maxY) { maxY = Y; }
            if (Z > maxZ) { maxZ = Z; }
            if (X < minX) { minX = X; }
            if (Y < minY) { minY = Y; }
            if (Z < minZ) { minZ = Z; }
        }

        private int ShiftSignedBits(uint input, int distance)
        {
            int signBit = 1 << 9; // 10 bits being selected
            int selectedBits = (int)((input >> distance) & 0x3FF);
            return (selectedBits ^ signBit) - signBit;
        }

        public void ReadFromCAR(Stream stream)
        {
            double scale = 4000; // No idea if this is correct, but it validates
            X = stream.ReadShort() / scale;
            Y = stream.ReadShort() / scale;
            Z = stream.ReadShort() / scale;
            Z = 0 - Z;
            stream.Position += sizeof(short);
            ValidateUnitVector();

            if (X > maxX) { maxX = X; }
            if (Y > maxY) { maxY = Y; }
            if (Z > maxZ) { maxZ = Z; }
            if (X < minX) { minX = X; }
            if (Y < minY) { minY = Y; }
            if (Z < minZ) { minZ = Z; }

        }

        private void ValidateUnitVector()
        {
            double total = Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
            if (total < 0.995 || total > 1.005)
            {
                throw new Exception("Invalid unit vector.");
            }
        }

        public void WriteToCDO(Stream stream)
        {
            double scale = 500; // From commongear's research
            uint i = UnshiftSignedBits((int)(X * scale), 2) + UnshiftSignedBits((int)(Y * scale), 12) + UnshiftSignedBits((int)(Z * scale), 22);
            stream.WriteUInt(i);
        }

        private uint UnshiftSignedBits(int input, int distance)
        {
            int signBit = 1 << 9;
            input = (input + signBit) ^ signBit;
            uint packedBits = (uint)(input << distance);
            return packedBits;
        }

        public void WriteToOBJ(TextWriter writer) => writer.WriteLine($"vn {X} {Y} {Z}");
    }
}