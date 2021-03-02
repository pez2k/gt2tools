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

        public void ReadFromCDO(Stream stream)
        {
            // 0110 1000 1000 1000 1010 0011 1010 1100
            // Z 01 1010 0010
            // Y 00 1000 1010
            // X 00 1110 1011
            // p 00
            double scale = 500; // From commongear's research
            uint i = stream.ReadUInt();
            X = ((i >> 2) & 0x3FF) / scale;
            Y = ((i >> 12) & 0x3FF) / scale;
            Z = ((i >> 22) & 0x3FF) / scale;

            if (X > maxX) { maxX = X; }
            if (Y > maxY) { maxY = Y; }
            if (Z > maxZ) { maxZ = Z; }
        }

        public void ReadFromCAR(Stream stream)
        {
            double scale = 32768; // No idea if this is correct!
            Z = stream.ReadUShort() / scale;
            Y = stream.ReadUShort() / scale;
            X = stream.ReadUShort() / scale;
            stream.Position += sizeof(ushort);

            if (X > maxX) { maxX = X; }
            if (Y > maxY) { maxY = Y; }
            if (Z > maxZ) { maxZ = Z; }
        }

        public void WriteToCDO(Stream stream)
        {
            double scale = 500; // From commongear's research
            uint i = ((uint)(X * scale) << 2) + ((uint)(Y * scale) << 12) + ((uint)(Z * scale) << 22);
            stream.WriteUInt(i);
        }
    }
}