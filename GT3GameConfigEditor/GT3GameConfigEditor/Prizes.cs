using System.IO;
using System.Text;
using CsvHelper;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    static class Prizes
    {
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
                        csv.WriteField("PrizeCount");
                        csv.WriteField("Unknown");
                        csv.WriteField("Event");
                        csv.WriteField("Unknown2");
                        csv.WriteField("Unknown3");
                        csv.WriteField("Unknown4");
                        csv.WriteField("Unknown5");
                        csv.WriteField("PrizeID");
                        csv.WriteField("Unknown6");
                        // prize
                        csv.WriteField("Colour");
                        csv.WriteField("Unknown7");
                        csv.WriteField("PrizeCar");
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
                            
                            uint structType = file.ReadUInt(); // always 0x0C?
                            uint prizeCount = file.ReadUInt();
                            uint unknown = file.ReadUInt();
                            string eventName = file.ReadCharacters();
                            long gap = file.Position % 4;
                            if (gap > 0)
                            {
                                file.Position += 4 - gap;
                            }
                            uint unknown2 = file.ReadUInt();
                            uint unknown3 = 0;
                            uint unknown4 = 0;
                            uint unknown5 = 0;
                            if (prizeCount > 1)
                            {
                                unknown3 = file.ReadUInt();
                            }
                            if (prizeCount > 2)
                            {
                                unknown4 = file.ReadUInt();
                            }
                            if (prizeCount > 3)
                            {
                                unknown5 = file.ReadUInt();
                            }

                            for (int j = 0; j < prizeCount; j++)
                            {
                                uint prizeID = file.ReadUInt();
                                uint unknown6 = file.ReadUInt(); // always 16?
                                uint colour = file.ReadUInt();
                                uint unknown7 = file.ReadUInt(); // always 1?
                                string prizeCar = file.ReadCharacters();
                                gap = file.Position % 4;
                                if (gap > 0)
                                {
                                    file.Position += 4 - gap;
                                }

                                csv.WriteField(prizeCount);
                                csv.WriteField(unknown);
                                csv.WriteField(eventName);
                                csv.WriteField(unknown2);
                                csv.WriteField(unknown3);
                                csv.WriteField(unknown4);
                                csv.WriteField(unknown5);
                                csv.WriteField(prizeID);
                                csv.WriteField(unknown6);
                                csv.WriteField(colour == 0xFFFFFFFF ? 0 : colour);
                                csv.WriteField(unknown7);
                                csv.WriteField(prizeCar);
                                csv.NextRecord();
                            }
                        }
                    }
                }
            }
        }
    }
}
