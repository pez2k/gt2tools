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

        public void ReadFromCAR(Stream stream)
        {
            stream.Position = 0x10;

            for (int i = 0; i < 4; i++)
            {
                var wheelPosition = new WheelPosition();
                wheelPosition.ReadFromCAR(stream);
                WheelPositions.Add(wheelPosition);
            }

            Unknown1 = stream.ReadUShort();
            Unknown2 = stream.ReadUShort();
            Unknown3 = stream.ReadUShort();
            Scale = stream.ReadUShort();

            stream.Position += 0x04;
            ushort lodCount = stream.ReadUShort();
            //
            lodCount = 1;
            //
            LODs = new List<LOD>(lodCount);

            stream.Position += 0x42;

            for (int i = 0; i < lodCount; i++)
            {
                var lod = new LOD();
                lod.ReadFromCAR(stream);
                LODs.Add(lod);
            }
        }

        public void WriteToCDO(Stream stream)
        {
            // GT header
            stream.Write(new byte[] { 0x47, 0x54, 0x02 });
            stream.Position = 0x18;
            stream.WriteUShort(Unknown1);
            stream.WriteUShort(Unknown2);
            stream.WriteUShort(Unknown3);
            stream.WriteUShort(Scale);

            foreach (WheelPosition wheelPosition in WheelPositions)
            {
                wheelPosition.WriteToCDO(stream);
            }

            stream.Position = 0x868;
            stream.WriteUShort((ushort)LODs.Count);

            stream.Position = 0x884;

            foreach (LOD lod in LODs)
            {
                lod.WriteToCDO(stream);
            }
        }
    }
}