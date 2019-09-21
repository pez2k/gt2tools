using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    static class Prizes
    {
        private struct PrizeData
        {
            public string Event;
            public uint PrizeID;
            public uint Unknown;
            public uint Colour;
            public uint Unknown2;
            public string PrizeCar;
        }

        private sealed class PrizeCSVMap : ClassMap<PrizeData>
        {
            public PrizeCSVMap()
            {
                Map(m => m.Event);
                Map(m => m.PrizeID);
                Map(m => m.Unknown);
                Map(m => m.Colour);
                Map(m => m.Unknown2);
                Map(m => m.PrizeCar);
            }
        }

        public static void Dump(Stream file, string directory, int fileNumber)
        {
            uint structureCount = file.ReadUInt();
            uint startOfIndexes = file.ReadUInt();
            using (var outFile = new FileStream(Path.Combine(directory, $"{fileNumber}_Prizes.csv"), FileMode.Create, FileAccess.Write))
            {
                using (TextWriter output = new StreamWriter(outFile, Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(output))
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.Configuration.RegisterClassMap<PrizeCSVMap>();
                        csv.WriteHeader<PrizeData>();
                        csv.NextRecord();

                        for (int i = 0; i < structureCount; i++)
                        {
                            file.Position = startOfIndexes + (i * 4);

                            uint structurePos = file.ReadUInt();

                            uint nextStructurePos;
                            if (i + 1 < structureCount)
                            {
                                nextStructurePos = file.ReadUInt();
                            }
                            else
                            {
                                nextStructurePos = (uint)file.Length;
                            }

                            uint structureSize = nextStructurePos - structurePos;

                            file.Position = structurePos;

                            file.ReadUInt(); // always 0x0C
                            uint prizeCount = file.ReadUInt();
                            uint prizeOffsetsOffset = file.ReadUInt(); // offset to start of prize indexes
                            string eventName = file.ReadCharacters();
                            file.Position = structurePos + prizeOffsetsOffset;
                            var prizeOffsets = new uint[prizeCount];
                            for (uint prizeNumber = 0; prizeNumber < prizeCount; prizeNumber++)
                            {
                                prizeOffsets[prizeNumber] = file.ReadUInt();
                            }

                            for (uint prizeNumber = 0; prizeNumber < prizeCount; prizeNumber++)
                            {
                                file.Position = structurePos + prizeOffsets[prizeNumber];

                                var data = new PrizeData();
                                data.Event = eventName;
                                data.PrizeID = file.ReadUInt();
                                data.Unknown = file.ReadUInt(); // always 0x10?
                                uint colour = file.ReadUInt(); // 0xFF FF FF FF if unset
                                data.Colour = colour == 0xFFFFFFFF ? 0 : colour;
                                data.Unknown2 = file.ReadUInt(); // always 1?
                                data.PrizeCar = file.ReadCharacters();

                                csv.WriteRecord(data);
                                csv.NextRecord();
                            }
                        }
                    }
                }
            }
        }
    }
}
