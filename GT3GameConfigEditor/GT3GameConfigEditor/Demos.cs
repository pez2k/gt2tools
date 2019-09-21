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
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public string Filename;
            public string Course;
        }

        private sealed class DemoCSVMap : ClassMap<DemoData>
        {
            public DemoCSVMap()
            {
                Map(m => m.Unknown);
                Map(m => m.Unknown2);
                Map(m => m.Unknown3);
                Map(m => m.Unknown4);
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
                        csv.WriteField("Unknown"); // start of course string
                        csv.WriteField("Unknown2"); // start of 4b gap after struct
                        csv.WriteField("Unknown3"); // always 0
                        csv.WriteField("Unknown4"); // IsLocked
                        csv.WriteField("Filename");
                        csv.WriteField("Course");
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
                            csv.WriteField(file.ReadUInt());
                            csv.WriteField(file.ReadUInt());
                            csv.WriteField(file.ReadUInt());
                            csv.WriteField(file.ReadUInt());
                            csv.WriteField(file.ReadCharacters());
                            long gap = file.Position % 4;
                            if (gap > 0)
                            {
                                file.Position += 4 - gap;
                            }
                            csv.WriteField(file.ReadCharacters());
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
                        output.WriteUInt(row.Unknown);
                        output.WriteUInt(row.Unknown2);
                        output.WriteUInt(row.Unknown3);
                        output.WriteUInt(row.Unknown4);
                        output.WriteCharacters(row.Filename);
                        long gap = output.Position % 4;
                        output.Position += 4 - gap;
                        output.WriteCharacters(row.Course);
                        gap = output.Position % 4;
                        output.Position += 4 - gap;
                        output.WriteUInt(0);

                        headerPosition += 4;
                        startOfData = (uint)(output.Position - startOfChunk);
                    }

                    output.Position = startOfChunk;
                    output.WriteUInt((uint)rows.Count());
                    output.Position = output.Length;
                }
            }
        }
    }
}
