using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    static class Courses
    {
        private struct CourseData
        {
            public string Course;
        }

        private sealed class CourseCSVMap : ClassMap<CourseData>
        {
            public CourseCSVMap()
            {
                Map(m => m.Course);
            }
        }

        public static void Dump(Stream file, string directory, int fileNumber)
        {
            uint structureCount = file.ReadUInt();
            uint startOfIndexes = file.ReadUInt();
            using (var outFile = new FileStream(Path.Combine(directory, $"{fileNumber}_Courses.csv"), FileMode.Create, FileAccess.Write))
            {
                using (TextWriter output = new StreamWriter(outFile, Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(output))
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.Configuration.RegisterClassMap<CourseCSVMap>();
                        csv.WriteHeader<CourseData>();
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

                            var data = new CourseData();
                            file.ReadUInt(); // always 0x04
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
                    csv.Configuration.RegisterClassMap<CourseCSVMap>();

                    var rows = new List<CourseData>();
                    while (csv.Read())
                    {
                        rows.Add(csv.GetRecord<CourseData>());
                    }

                    long startOfChunk = output.Position;
                    output.WriteUInt((uint)rows.Count);
                    output.WriteUInt(8);

                    long headerPosition = output.Position;
                    uint startOfData = (uint)((rows.Count * 4) + 8);

                    foreach (CourseData row in rows)
                    {
                        output.Position = headerPosition;
                        output.WriteUInt(startOfData);
                        output.Position = startOfChunk + startOfData;
                        output.WriteUInt(0x04);
                        output.WriteCharacters(row.Course);
                        long gap = output.Position % 4;
                        output.Position += 4 - gap;
                        output.SetLength(output.Position);

                        headerPosition += 4;
                        startOfData = (uint)(output.Length - startOfChunk);
                    }
                }
            }
        }
    }
}
