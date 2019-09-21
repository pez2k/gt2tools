using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    static class CarClasses
    {
        private struct CarClassData
        {
            public ushort Unknown;
            public ushort UnlockLevel;
            public ushort OpponentGroup;
            public ushort Unknown2;
            public string CarName;
            public string EventRestriction;
        }

        private sealed class CarClassCSVMap : ClassMap<CarClassData>
        {
            public CarClassCSVMap()
            {
                Map(m => m.Unknown);
                Map(m => m.UnlockLevel);
                Map(m => m.OpponentGroup);
                Map(m => m.Unknown2);
                Map(m => m.CarName);
                Map(m => m.EventRestriction);
            }
        }

        public static void Dump(Stream file, string directory, int fileNumber)
        {
            uint outerStructureCount = file.ReadUInt();
            uint outerStartOfIndexes = file.ReadUInt();

            for (int i = 0; i < outerStructureCount; i++)
            {
                file.Position = outerStartOfIndexes + (i * 4);

                uint outerStructurePos = file.ReadUInt();

                file.Position = outerStructurePos;

                file.ReadUInt(); // always 0x0C
                uint structureCount = file.ReadUInt();
                uint startOfIndexes = file.ReadUInt();
                string listName = file.ReadCharacters();

                using (var outFile = new FileStream(Path.Combine(directory, $"{fileNumber}_CarClasses_{listName}.csv"), FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter output = new StreamWriter(outFile, Encoding.UTF8))
                    {
                        using (var csv = new CsvWriter(output))
                        {
                            csv.Configuration.QuoteAllFields = true;
                            csv.Configuration.RegisterClassMap<CarClassCSVMap>();
                            csv.WriteHeader<CarClassData>();
                            csv.NextRecord();

                            for (int j = 0; j < structureCount; j++)
                            {
                                file.Position = startOfIndexes + (j * 4) + outerStructurePos;

                                uint structurePos = file.ReadUInt();
                                file.Position = structurePos + outerStructurePos;

                                var data = new CarClassData();
                                file.ReadUInt(); // always 0x10
                                uint secondStringOffset = file.ReadUInt();
                                data.Unknown = file.ReadUShort();
                                data.UnlockLevel = file.ReadUShort();
                                data.OpponentGroup = file.ReadUShort();
                                data.Unknown2 = file.ReadUShort();
                                data.CarName = file.ReadCharacters();
                                file.Position = structurePos + outerStructurePos + secondStringOffset;
                                data.EventRestriction = file.ReadCharacters();
                                csv.WriteRecord(data);
                                csv.NextRecord();
                            }
                        }
                    }
                }
            }
        }

        public static void Import(Stream output, List<string> filePaths)
        {
            long startOfOuterChunk = output.Position;
            output.WriteUInt((uint)filePaths.Count);
            output.WriteUInt(8);

            long outerHeaderPosition = output.Position;
            uint startOfOuterData = (uint)((filePaths.Count * 4) + 8);

            foreach (string filePath in filePaths)
            {
                output.Position = outerHeaderPosition;
                output.WriteUInt(startOfOuterData);
                output.Position = startOfOuterChunk + startOfOuterData;

                using (var csvFile = new StreamReader(filePath, Encoding.UTF8))
                {
                    using (var csv = new CsvReader(csvFile))
                    {
                        csv.Configuration.RegisterClassMap<CarClassCSVMap>();

                        var rows = new List<CarClassData>();
                        while (csv.Read())
                        {
                            rows.Add(csv.GetRecord<CarClassData>());
                        }

                        long startOfChunk = output.Position;
                        output.WriteUInt(0x0C);
                        output.WriteUInt((uint)rows.Count);
                        long carOffsetsOffsetPosition = output.Position;
                        output.WriteUInt(0);
                        output.WriteCharacters(Path.GetFileNameWithoutExtension(filePath).Substring(13));
                        long gap = output.Position % 4;
                        output.Position += 4 - gap;
                        uint carOffsetsOffset = (uint)(output.Position - startOfChunk);
                        output.SetLength(output.Position);
                        output.Position = carOffsetsOffsetPosition;
                        output.WriteUInt(carOffsetsOffset);

                        long headerPosition = output.Length;
                        uint startOfData = (uint)(rows.Count * 4);

                        foreach (CarClassData row in rows)
                        {
                            output.Position = headerPosition;
                            output.WriteUInt(startOfData + carOffsetsOffset);
                            output.Position = startOfChunk + startOfData + carOffsetsOffset;
                            output.WriteUInt(0x10);
                            long secondStringOffsetPosition = output.Position;
                            output.WriteUInt(0);
                            output.WriteUShort(row.Unknown);
                            output.WriteUShort(row.UnlockLevel);
                            output.WriteUShort(row.OpponentGroup);
                            output.WriteUShort(row.Unknown2);
                            output.WriteCharacters(row.CarName);
                            gap = output.Position % 4;
                            output.Position += 4 - gap;
                            uint secondStringOffset = (uint)(output.Position - startOfChunk - startOfData - carOffsetsOffset);
                            output.WriteCharacters(row.EventRestriction);
                            gap = output.Position % 4;
                            output.SetLength(output.Position + 4 - gap);

                            output.Position = secondStringOffsetPosition;
                            output.WriteUInt(secondStringOffset);

                            headerPosition += 4;
                            startOfData = (uint)(output.Length - startOfChunk - carOffsetsOffset);
                        }
                    }
                }

                outerHeaderPosition += 4;
                startOfOuterData = (uint)(output.Length - startOfOuterChunk);
            }
            output.Position = output.Length;
        }
    }
}
