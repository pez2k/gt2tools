using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StreamExtensions;

namespace GT1.SpecSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No filename provided.");
                return;
            }
            Dump(args[0]);
        }

        private static void Dump(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Position = 4; // skip header
                string fileType = file.ReadCharacters();

                string directory = $"{Path.GetFileNameWithoutExtension(filename)}_{Path.GetExtension(filename)}_{fileType}";
                Directory.CreateDirectory(directory);

                file.Position = 0x0C;
                
                file.ReadUShort(); // always 0x10?
                List<byte[]> structs = ReadMainStructs(file);
                List<List<string>> stringTables = null;
                if (file.Position < file.Length)
                {
                    stringTables = ReadStringTables(file);
                }
                ExportData(structs, stringTables, fileType, directory);
            }
        }

        private static List<byte[]> ReadMainStructs(Stream file)
        {
            ushort structCount = file.ReadUShort();
            file.ReadUInt(); // always zero?
            uint structSize = file.ReadUInt();

            var structs = new List<byte[]>(structCount);
            for (ushort i = 0; i < structCount; i++)
            {
                var buffer = new byte[structSize];
                file.Read(buffer);
                structs.Add(buffer);
            }
            return structs;
        }

        private static List<List<string>> ReadStringTables(Stream file)
        {
            uint tableCount = file.ReadUInt();
            file.Position += tableCount * 4;
            var strings = new List<List<string>>((int)tableCount);

            for (uint i = 0; i < tableCount; i++)
            {
                strings.Add(ReadStrings(file));
            }
            return strings;
        }

        private static List<string> ReadStrings(Stream file)
        {
            ushort stringCount = file.ReadUShort();
            var strings = new List<string>(stringCount);
            for (ushort i = 0; i < stringCount; i++)
            {
                byte stringLength = file.ReadSingleByte();
                var buffer = new byte[stringLength];
                file.Read(buffer);
                string textString = Encoding.ASCII.GetString(buffer);
                strings.Add(textString);
                file.Position++;
            }
            if (file.Position % 2 > 0)
            {
                file.Position++;
            }
            return strings;
        }

        private static void ExportData(List<byte[]> structs, List<List<string>> stringTables, string fileType, string directory)
        {
            if (fileType == "COLOR")
            {
                DumpColourData(structs, stringTables, directory);
                return;
            }

            DumpStructs(structs, directory, fileType);
            DumpStringTables(stringTables, directory);
        }

        private static void DumpStructs(List<byte[]> structs, string directory, string fileType)
        {
            int i = 0;
            foreach (var buffer in structs)
            {
                string outputName = $"{i:D4}";
                switch (fileType)
                {
                    case "SPEC":
                        outputName += $"_{Encoding.ASCII.GetString(buffer, 0, 5)}";
                        break;
                    case "EQUIP":
                        outputName += $"_{Encoding.ASCII.GetString(buffer, 0x60, 7)}";
                        break;
                }
                using (var output = new FileStream($"{directory}\\{outputName}.dat", FileMode.Create, FileAccess.Write))
                {
                    output.Write(buffer);
                }
                i++;
            }
        }

        private static void DumpStringTables(List<List<string>> stringTables, string directory)
        {
            for (int i = 0; i < stringTables.Count; i++)
            {
                using (var output = new StreamWriter($"{directory}\\strings{i}.txt"))
                {
                    foreach (string textString in stringTables[i])
                    {
                        output.WriteLine(textString);
                    }
                    output.Flush();
                }
            }
        }

        private static void DumpColourData(List<byte[]> structs, List<List<string>> stringTables, string directory)
        {
            using (var output = File.CreateText($"{directory}\\Colours.csv"))
            {
                output.WriteLine($"\"CarID\",\"ColourID\",\"ColourName\"");

                foreach (byte[] structure in structs)
                {
                    using (var stream = new MemoryStream(structure))
                    {
                        ushort carID = stream.ReadUShort();
                        for (int i = 0; i < 16; i++)
                        {
                            stream.Position = i + 2;
                            byte colourID = stream.ReadSingleByte();
                            stream.Position = (i * 4) + 20;
                            ushort stringNumber = stream.ReadUShort();
                            ushort tableNumber = stream.ReadUShort();
                            string colourName = stringTables[tableNumber][stringNumber];
                            if (colourID > 0)
                            {
                                output.WriteLine($"\"{carID}\",\"{colourID:X2}\",\"{colourName}\"");
                            }
                        }
                    }
                }
                output.Flush();
            }
        }
    }
}
