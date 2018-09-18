using System.Collections.Generic;
using System.IO;

namespace GT2.ModelTool.Structures
{
    using StreamExtensions;

    public class Model
    {
        public ushort Unknown1 { get; set; }
        public ushort Unknown2 { get; set; }
        public ushort Unknown3 { get; set; }
        public ushort Scale { get; set; }
        public List<WheelPosition> WheelPositions { get; set; } = new List<WheelPosition>(4);
        public List<LOD> LODs { get; set; }

        public void ReadFromCDO(Stream stream) {
            stream.Position = 0x08;
            Unknown1 = stream.ReadUShort();
            if (Unknown1 == 0) {
                stream.Position = 0x18;
                Unknown1 = stream.ReadUShort();
            }

            Unknown2 = stream.ReadUShort();
            Unknown3 = stream.ReadUShort();
            Scale = stream.ReadUShort();

            for (int i = 0; i < 4; i++) {
                var wheelPosition = new WheelPosition();
                wheelPosition.ReadFromCDO(stream);
                WheelPositions.Add(wheelPosition);
            }

            stream.Position += 0x828;
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