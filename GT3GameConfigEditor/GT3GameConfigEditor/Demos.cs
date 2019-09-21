using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    static class Demos
    {
        private struct DemoData
        {
            public uint Unknown;
            public uint IsLocked;
            public string Filename;
            public string Course;
        }

        private sealed class DemoCSVMap : ClassMap<DemoData>
        {
            public DemoCSVMap()
            {
                Map(m => m.Unknown);
                Map(m => m.IsLocked);
                Map(m => m.Filename);
                Map(m => m.Course);
            }
        }

        public static void Dump(Stream file, string directory, int fileNumber)
        {
            uint structureCount = file.ReadUInt();
            uint startOfIndexes = file.ReadUInt();
            using (var outFile = new FileStream(Path.Combine(directory, $"{fileNumber}_Demos.csv"), FileMode.Create, FileAccess.Write))
            {
                using (TextWriter output = new StreamWriter(outFile, Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(output))
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.Configuration.RegisterClassMap<DemoCSVMap>();
                        csv.WriteHeader<DemoData>();
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
                            
                            file.ReadUInt(); // always 0x14

                            var data = new DemoData();
                            uint secondStringOffset = file.ReadUInt();
                            file.ReadUInt(); // end of struct offset - ignore it
                            data.Unknown = file.ReadUInt();
                            data.IsLocked = file.ReadUInt();
                            data.Filename = file.ReadCharacters();
                            file.Position = structurePos + secondStringOffset;
                            data.Course = file.ReadCharacters();

                            csv.WriteRecord(data);
                            csv.NextRecord();
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
                    long startOfChunk = output.Position;

                    output.WriteUInt(0);
                    output.WriteUInt(8);

                    csv.Configuration.RegisterClassMap<DemoCSVMap>();

                    var rows = new List<DemoData>();
                    while (csv.Read())
                    {
                        rows.Add(csv.GetRecord<DemoData>());
                    }

                    long headerPosition = output.Position;
                    uint startOfData = (uint)((rows.Count() * 4) + 8);

                    foreach (DemoData row in rows)
                    {
                        output.Position = headerPosition;
                        output.WriteUInt(startOfData);
                        output.Position = startOfChunk + startOfData;
                        output.WriteUInt(0x14);
                        output.WriteUInt(0);
                        output.WriteUInt(0);
                        output.WriteUInt(row.Unknown);
                        output.WriteUInt(row.IsLocked);
                        output.WriteCharacters(row.Filename);
                        long gap = output.Position % 4;
                        output.Position += 4 - gap;
                        uint secondStringStart = (uint)(output.Position - startOfChunk - startOfData);
                        output.WriteCharacters(row.Course);
                        gap = output.Position % 4;
                        output.Position += 4 - gap;
                        uint paddingStart = (uint)(output.Position - startOfChunk - startOfData);
                        output.WriteUInt(0);

                        output.Position = startOfChunk + startOfData + 4;
                        output.WriteUInt(secondStringStart);
                        output.WriteUInt(paddingStart);

                        headerPosition += 4;
                        startOfData = (uint)(output.Length - startOfChunk);
                    }

                    output.Position = startOfChunk;
                    output.WriteUInt((uint)rows.Count());
                    output.Position = output.Length;
                }
            }
        }
    }
}
