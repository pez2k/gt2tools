using System.Collections.Generic;
using System.IO;
using System.Linq; 

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
            double x = X * Vertex.UnitsToMetres;
            double y = Y * Vertex.UnitsToMetres;
            double z = Z * Vertex.UnitsToMetres;
            writer.WriteLine($"g wheelpos{wheelNumber}");
            writer.WriteLine($"# True wheel position is the centre point of the four vertices below");
            writer.WriteLine($"v {x} {y + 0.01} {z - 0.01}");
            writer.WriteLine($"v {x} {y + 0.01} {z + 0.01}");
            writer.WriteLine($"v {x} {y - 0.01} {z + 0.01}");
            writer.WriteLine($"v {x} {y - 0.01} {z - 0.01}");
            writer.WriteLine($"usemtl untextured");
            writer.WriteLine($"f {vertexNumber++} {vertexNumber++} {vertexNumber++} {vertexNumber++}");
            menuWheelOffsets.Add((short)(X - MenuX));
        }

        public void ReadFromOBJ(List<Vertex> vertices, short menuXOffset)
        {
            // Use the centre of all wheel position vertices for the actual position
            X = (short)vertices.Average(vertex => vertex.X);
            Y = (short)vertices.Average(vertex => vertex.Y);
            Z = (short)vertices.Average(vertex => vertex.Z);
            MenuX = (short)(X - menuXOffset);
        }
    }
}