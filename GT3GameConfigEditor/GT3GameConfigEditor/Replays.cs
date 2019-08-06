﻿using System.IO;
using System.Text;
using CsvHelper;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    static class Replays
    {
        public static void Dump(Stream file, string directory, int fileNumber)
        {
            uint structureCount = file.ReadUInt();
            uint startOfIndexes = file.ReadUInt();
            using (var outFile = new FileStream(Path.Combine(directory, $"{fileNumber}_Replays.csv"), FileMode.Create, FileAccess.Write))
            {
                using (TextWriter output = new StreamWriter(outFile, Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(output))
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.WriteField("Unknown");
                        csv.WriteField("Filename");
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
                            
                            file.ReadUInt(); // always 0x08
                            csv.WriteField(file.ReadUInt());
                            csv.WriteField(file.ReadCharacters());
                            csv.NextRecord();
                        }
                    }
                }
            }
        }
    }
}