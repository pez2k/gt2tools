using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class WheelPosition
    {
        public short X { get; set; } // track width in race, ignored for right wheels as a shortcut
        public short Y { get; set; } // vertical
        public short Z { get; set; } // forwards / backwards
        public short MenuX { get; set; } // track width in menus, all four used - circa 0.875 * X

        public void ReadFromCDO(Stream stream)
        {
            X = stream.ReadShort();
            Y = stream.ReadShort();
            Z = stream.ReadShort();
            MenuX = stream.ReadShort();
        }

        public void ReadFromCAR(Stream stream)
        {
            ReadFromCDO(stream);
            MenuX = X; // zero in GT1, using the X value isn't totally correct but better than nothing
        }

        public void WriteToCDO(Stream stream)
        {
            stream.WriteShort(X);
            stream.WriteShort(Y);
            stream.WriteShort(Z);
            stream.WriteShort(MenuX);
        }

        public void WriteToOBJ(TextWriter writer, int wheelNumber, int vertexNumber, List<short> menuWheelOffsets)
        {
            writer.WriteLine($"g wheelpos{wheelNumber}/w={MenuX}");
            writer.WriteLine($"v {X * Vertex.UnitsToMetres} {Y * Vertex.UnitsToMetres} {Z * Vertex.UnitsToMetres} {MenuX * Vertex.UnitsToMetres}");
            writer.WriteLine($"f {vertexNumber} {vertexNumber} {vertexNumber}");
            menuWheelOffsets.Add((short)(X - MenuX));
        }

        public void ReadFromOBJ(Vertex vertex, short wValue)
        {
            X = vertex.X;
            Y = vertex.Y;
            Z = vertex.Z;
            MenuX = vertex.W == 0 ? wValue : vertex.W;
        }
    }
}