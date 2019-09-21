using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    static class Events
    {
        private struct EventData
        {
            public uint IsLocked;
            public string Event;
        }

        private sealed class EventCSVMap : ClassMap<EventData>
        {
            public EventCSVMap()
            {
                Map(m => m.IsLocked);
                Map(m => m.Event);
            }
        }

        public static void Dump(Stream file, string directory, int fileNumber)
        {
            uint structureCount = file.ReadUInt();
            uint startOfIndexes = file.ReadUInt();
            using (var outFile = new FileStream(Path.Combine(directory, $"{fileNumber}_Events.csv"), FileMode.Create, FileAccess.Write))
            {
                using (TextWriter output = new StreamWriter(outFile, Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(output))
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.Configuration.RegisterClassMap<EventCSVMap>();
                        csv.WriteHeader<EventData>();
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

                            var data = new EventData();
                            file.ReadUInt(); // always 0x08
                            data.IsLocked = file.ReadUInt();
                            data.Event = file.ReadCharacters();
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
                    csv.Configuration.RegisterClassMap<EventCSVMap>();

                    var rows = new List<EventData>();
                    while (csv.Read())
                    {
                        rows.Add(csv.GetRecord<EventData>());
                    }

                    long startOfChunk = output.Position;
                    output.WriteUInt((uint)rows.Count);
                    output.WriteUInt(8);

                    long headerPosition = output.Position;
                    uint startOfData = (uint)((rows.Count * 4) + 8);

                    foreach (EventData row in rows)
                    {
                        output.Position = headerPosition;
                        output.WriteUInt(startOfData);
                        output.Position = startOfChunk + startOfData;
                        output.WriteUInt(0x08);
                        output.WriteUInt(row.IsLocked);
                        output.WriteCharacters(row.Event);
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
