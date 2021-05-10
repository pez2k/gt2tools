using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class WheelPosition
    {
        public short X { get; set; }
        public short Y { get; set; } // vertical
        public short Z { get; set; } // forwards / backwards
        public short W { get; set; } // track width? probably signed?

        public void ReadFromCDO(Stream stream)
        {
            X = stream.ReadShort(); // these are probably in the wrong order
            Y = stream.ReadShort();
            Z = stream.ReadShort();
            W = stream.ReadShort();
        }

        public void ReadFromCAR(Stream stream)
        {
            ReadFromCDO(stream);
            W = X; // zero in GT1, using the X value isn't totally correct but better than nothing
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteShort(X);
            stream.WriteShort(Y);
            stream.WriteShort(Z);
            stream.WriteShort(W);
        }

        public void WriteToOBJ(TextWriter writer, int wheelNumber)
        {
            writer.WriteLine($"g wheelpos{wheelNumber}/w={W}");
            writer.WriteLine($"v {X * Vertex.UnitsToMetres} {Y * Vertex.UnitsToMetres} {Z * Vertex.UnitsToMetres} {W * Vertex.UnitsToMetres}");
            wheelNumber++;
            writer.WriteLine($"f {wheelNumber} {wheelNumber} {wheelNumber}");
        }

        public void ReadFromOBJ(Vertex vertex, short wValue)
        {
            X = vertex.X;
            Y = vertex.Y;
            Z = vertex.Z;
            W = vertex.Padding == 0 ? wValue : vertex.Padding;
        }
    }
}