using System.IO;
using System.Text;
using CsvHelper;
using StreamExtensions;

namespace GT3.GameConfigEditor
{
    static class CarClasses
    {
        public static void Dump(Stream file, string directory, int fileNumber)
        {
            uint outerStructureCount = file.ReadUInt();
            uint outerStartOfIndexes = file.ReadUInt();

            for (int i = 0; i < outerStructureCount; i++)
            {
                file.Position = outerStartOfIndexes + (i * 4);

                uint outerStructurePos = file.ReadUInt();

                file.Position = outerStructurePos;

                file.ReadUInt();// always 0x0C
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
                            csv.WriteField("Unknown");
                            csv.WriteField("UnlockLevel");
                            csv.WriteField("Unknown2");
                            csv.WriteField("Unknown3");
                            csv.WriteField("Name");
                            csv.NextRecord();

                            for (int j = 0; j < structureCount; j++)
                            {
                                file.Position = startOfIndexes + (j * 4) + outerStructurePos;

                                uint structurePos = file.ReadUInt();
                                file.Position = structurePos + outerStructurePos;

                                file.ReadUInt(); // always 0x10
                                file.ReadUInt(); // struct size?
                                csv.WriteField(file.ReadUShort());
                                csv.WriteField(file.ReadUShort());
                                csv.WriteField(file.ReadUShort());
                                csv.WriteField(file.ReadUShort());
                                csv.WriteField(file.ReadCharacters());
                                csv.NextRecord();
                            }
                        }
                    }
                }
            }
        }
    }
}
