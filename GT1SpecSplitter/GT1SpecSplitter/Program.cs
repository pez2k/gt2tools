using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StreamExtensions;

namespace GT1.SpecSplitter
{
    class Program
    {
        private const int Windows31J = 932; // Windows-31J, code page 932

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No filename provided.");
            }
            else if (File.GetAttributes(args[0]).HasFlag(FileAttributes.Directory))
            {
                Rebuild(args[0]);
            }
            else
            {
                Dump(args[0]);
            }
        }

        private static void Dump(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Position = 4; // skip header
                string fileType = file.ReadCharacters();

                string directory = $"{Path.GetFileNameWithoutExtension(filename)}_{fileType}";
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
                string textString = Encoding.GetEncoding(Windows31J).GetString(buffer);
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
                        outputName = $"{i + 1:D3}_{Encoding.ASCII.GetString(buffer, 0x60, 7)}";
                        break;
                }
                using (var output = new FileStream($"{directory}\\{outputName}.dat", FileMode.Create, FileAccess.Write))
                {
                    output.Write(buffer);
                }
                i++;

                // DISPLAC - 0x10 - car ID
                // LWEIGHT - 0x03 - car ID
                // EQUIP - 0x00 - car ID - 0x08 - RM ID? - 0x2A - tyre ID
                // FLYWHEL - 0x04 - car ID - 0x06 - stage - 0x07 - stage = 0x08 - price
                // RACING - 0x0E - part ID? - 0x10 - price - 0x14 - string ID 1 - 0x18 - string ID 2
                // SPEC - 0x184 - car price

                // 0000.equip - All cars, minus C2 and NB
                // 0001.equip - All cars - tyres equipped
                // 0002.equip - All cars - different tyres equipped
                // 0003.equip - Spot Race opponents? No racecars or bonus cars, but C2 and NB included
                // 0004.equip - FF / FR / 4WD Challenge opponents
                // 0005.equip - UK v US v Japan opponents
                // 0006.equip - Lightweight Sports / Normal Car opponents
                // 0007.equip - Hard-Tuned Car & SSR11 All Night II opponents
                // 0008.equip - Megaspeed Cup opponents
                // 0009.equip - Sunday Cup opponents
                // 0010.equip - Clubman Cup opponents
                // 0011.equip - Gran Turismo Cup opponents
                // 0012.equip - GT World Cup & Grand Valley 300km & SSR11 All Night I opponents
            }
        }

        private static void DumpStringTables(List<List<string>> stringTables, string directory)
        {
            for (int i = 0; i < stringTables.Count; i++)
            {
                using (var output = new StreamWriter($"{directory}\\strings{i}.txt", false, Encoding.UTF8))
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

        private static void Rebuild(string path)
        {
            string[] parts = Path.GetFileName(path).Split('_');
            string fileType = parts.Last();
            if (string.IsNullOrWhiteSpace(fileType) || fileType.Length > 7)
            {
                throw new Exception($"Invalid file type: {fileType}");
            }
            string filename = "new_" + Path.GetFileName(path).Replace($"_{fileType}", $".{fileType.ToLower()}");

            using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("@(#)");
                file.WriteCharacters(fileType);
                file.Position = 0x0C;
                file.WriteUShort(0x10); // always 0x10?
                List<byte[]> structs = ImportStructs(path);
                List<List<string>> stringTables = ImportStringTables(path);
                WriteData(structs, stringTables, fileType, file);
            }
        }

        private static List<byte[]> ImportStructs(string directory)
        {
            string[] files = Directory.EnumerateFiles(directory, "*.dat").ToArray();
            var structs = new List<byte[]>(files.Length);
            foreach (string file in files)
            {
                using (var input = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[input.Length];
                    input.Read(buffer);
                    structs.Add(buffer);
                }
            }
            return structs;
        }

        private static List<List<string>> ImportStringTables(string directory)
        {
            string[] files = Directory.EnumerateFiles(directory, "strings*.txt").ToArray();
            var stringTables = new List<List<string>>(files.Length);
            foreach (string file in files)
            {
                using (var input = new StreamReader(file, Encoding.UTF8))
                {
                    var strings = new List<string>();
                    while (!input.EndOfStream)
                    {
                        strings.Add(input.ReadLine());
                    }
                    stringTables.Add(strings);
                }
            }
            return stringTables;
        }

        private static void WriteData(List<byte[]> structs, List<List<string>> stringTables, string fileType, Stream file)
        {
            if (fileType == "COLOR")
            {
                throw new NotImplementedException();
            }

            WriteMainStructs(structs, file);
            WriteStringTables(stringTables, file);
        }

        private static void WriteMainStructs(List<byte[]> structs, Stream file)
        {
            int structCount = structs.Count;
            file.WriteUShort((ushort)structCount);
            file.WriteUInt(0); // always zero?
            int structSize = structs.Any() ? structs[0].Length : 0;
            file.WriteUInt((uint)structSize);

            foreach (byte[] structure in structs)
            {
                file.Write(structure);
            }
        }

        private static void WriteStringTables(List<List<string>> stringTables, Stream file)
        {
            int tableCount = stringTables.Count;
            file.WriteUInt((uint)tableCount);
            file.Position += tableCount * 4;

            foreach (List<string> strings in stringTables)
            {
                WriteStrings(strings, file);
            }
        }

        private static void WriteStrings(List<string> strings, Stream file)
        {
            int stringCount = strings.Count;
            file.WriteUShort((ushort)stringCount);
            foreach (string textString in strings)
            {
                byte[] textData = Encoding.GetEncoding(Windows31J).GetBytes(textString);
                int stringLength = textData.Length;
                file.WriteByte((byte)stringLength);
                file.Write(textData);
                file.WriteByte(0);
            }
            if (file.Position % 2 > 0)
            {
                file.WriteByte(0);
            }
        }
    }
}
