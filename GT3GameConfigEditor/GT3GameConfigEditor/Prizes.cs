using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static void Import(Stream output, string filePath)
        {
            using (var csvFile = new StreamReader(filePath, Encoding.UTF8))
            {
                using (var csv = new CsvReader(csvFile))
                {
                    csv.Configuration.RegisterClassMap<PrizeCSVMap>();

                    var rows = new List<PrizeData>();
                    while (csv.Read())
                    {
                        rows.Add(csv.GetRecord<PrizeData>());
                    }
                    rows = rows.OrderBy(row => row.PrizeID).ToList();
                    List<string> eventNames = rows.Select(row => row.Event).Distinct().OrderBy(eventName => eventName, StringComparer.Ordinal).ToList();

                    long startOfChunk = output.Position;
                    output.WriteUInt((uint)eventNames.Count);
                    output.WriteUInt(8);

                    long headerPosition = output.Position;
                    uint startOfData = (uint)((eventNames.Count * 4) + 8);

                    foreach (string eventName in eventNames)
                    {
                        List<PrizeData> prizes = rows.Where(row => row.Event == eventName).ToList();

                        output.Position = headerPosition;
                        output.WriteUInt(startOfData);
                        output.Position = startOfChunk + startOfData;
                        output.WriteUInt(0x0C);
                        output.WriteUInt((uint)prizes.Count);
                        long prizeOffsetsOffsetPosition = output.Position;
                        output.WriteUInt(0);
                        output.WriteCharacters(eventName);
                        long gap = output.Position % 4;
                        long prizeOffsetsOffset = output.Position + 4 - gap;
                        output.Position = prizeOffsetsOffsetPosition;
                        output.WriteUInt((uint)(prizeOffsetsOffset - startOfChunk - startOfData));
                        output.Position = prizeOffsetsOffset + (prizes.Count * 4);

                        uint prizeNumber = 0;
                        foreach (PrizeData prize in prizes)
                        {
                            long prizeOffset = output.Position;
                            output.Position = prizeOffsetsOffset + (prizeNumber * 4);
                            output.WriteUInt((uint)(prizeOffset - startOfChunk - startOfData));
                            output.Position = prizeOffset;

                            output.WriteUInt(prize.PrizeID);
                            output.WriteUInt(prize.Unknown);
                            output.WriteUInt(prize.Colour == 0 ? 0xFFFFFFFF : prize.Colour);
                            output.WriteUInt(prize.Unknown2);
                            output.WriteCharacters(prize.PrizeCar);
                            gap = output.Position % 4;
                            output.Position += 4 - gap;

                            prizeNumber++;
                        }

                        headerPosition += 4;
                        startOfData = (uint)(output.Position - startOfChunk);
                    }
                }
            }
        }
    }
}
