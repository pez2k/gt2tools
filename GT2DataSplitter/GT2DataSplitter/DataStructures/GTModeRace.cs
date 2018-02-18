using System.Collections.Generic;
using System.IO;

namespace GT2DataSplitter
{
    public class GTModeRace
    {
        public List<Race> Races { get; set; } = new List<Race>();
        public List<Opponent> Opponents { get; set; } = new List<Opponent>();
        public List<RaceUnknown1> RaceUnknown1 { get; set; } = new List<RaceUnknown1>();
        public List<RaceStrings> RaceStrings { get; set; } = new List<RaceStrings>();
        
        public void ReadData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var blocks = new List<DataBlock>();

                for (int i = 1; i <= 31; i++)
                {
                    file.Position = 8 * i;
                    uint blockStart = file.ReadUInt();
                    uint blockSize = file.ReadUInt();
                    blocks.Add(new DataBlock { BlockStart = blockStart, BlockSize = blockSize });
                }

                Races.Read(file, blocks[0].BlockStart, blocks[0].BlockSize);
                Opponents.Read(file, blocks[1].BlockStart, blocks[1].BlockSize);
                RaceUnknown1.Read(file, blocks[2].BlockStart, blocks[2].BlockSize);
                RaceStrings.Read(file, blocks[3].BlockStart, blocks[3].BlockSize);
            }
        }

        public void DumpData()
        {
            Races.Dump();
            Opponents.Dump();
            RaceUnknown1.Dump();
            RaceStrings.Dump();
        }

        public struct DataBlock
        {
            public uint BlockStart;
            public uint BlockSize;
        }
    }
}
