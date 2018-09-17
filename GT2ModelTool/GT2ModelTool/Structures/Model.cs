using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class Model
    {
        public ushort Scale { get; set; }
        public List<WheelPosition> WheelPositions { get; set; } = new List<WheelPosition>(4);
        public List<LOD> LODs { get; set; }

        public void ReadFromCDO(Stream stream) {
            ushort dataStart = 0x868;
            stream.Position = 0x08;
            if (stream.ReadByte() > 0) {
                dataStart -= 0x10;
            }

            stream.Position = 0x1E;
            Scale = stream.ReadUShort();

            stream.Position = dataStart;
            ushort lodCount = stream.ReadUShort();
            LODs = new List<LOD>(lodCount);

            stream.Position += 0x1A;

            for (int i = 0; i < lodCount; i++)
            {
                var lod = new LOD();
                lod.ReadFromCDO(stream);
                LODs.Add(lod);
            }
        }
    }
}