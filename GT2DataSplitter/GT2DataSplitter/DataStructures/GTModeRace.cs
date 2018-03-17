using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace GT2.DataSplitter
{
    using StreamExtensions;

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

        public void ImportData()
        {
            Races.Import();
            Opponents.Import();
            RaceUnknown1.Import();
            RaceStrings.Import();
        }

        public void WriteData(string filename)
        {
            filename = "new_" + filename;

            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                file.Write(new byte[] { 0x47, 0x54, 0x44, 0x54, 0x6C, 0x00, 0x06, 0x00 }, 0, 8); // The 0x06 is the number of indices

                file.Position = (0x06 * 8) + 7;
                file.WriteByte(0x00); // Data starts at 0x38 so position EOF

                uint i = 1;
                Races.Write(file, 8 * i++);
                Opponents.Write(file, 8 * i++);
                RaceUnknown1.Write(file, 8 * i++);
                RaceStrings.Write(file, 8 * i++);

                file.Position = 0;
                using (FileStream zipFile = new FileStream(filename + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new GZipStream(zipFile, CompressionMode.Compress))
                    {
                        file.CopyTo(zip);
                    }
                }
            }
        }

        public struct DataBlock
        {
            public uint BlockStart;
            public uint BlockSize;
        }
    }
}
