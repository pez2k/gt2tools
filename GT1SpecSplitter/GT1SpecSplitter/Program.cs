using System;
using System.Collections.Generic;
using System.Globalization;
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
                return;
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Required to support code pages, including 932

            if (File.GetAttributes(args[0]).HasFlag(FileAttributes.Directory))
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
            using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
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

            List<byte[]> structs = new(structCount);
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
            List<List<string>> strings = new((int)tableCount);

            for (uint i = 0; i < tableCount; i++)
            {
                strings.Add(ReadStrings(file));
            }
            return strings;
        }

        private static List<string> ReadStrings(Stream file)
        {
            ushort stringCount = file.ReadUShort();
            List<string> strings = new(stringCount);
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

            DumpStructs(structs, directory, fileType, stringTables);
            DumpStringTables(stringTables, directory);
        }

        private static void DumpStructs(List<byte[]> structs, string directory, string fileType, List<List<string>> stringTables)
        {
            int i = 0;
            foreach (byte[] buffer in structs)
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
                    case "RACING":
                        outputName += $"_{stringTables[0][buffer[0x14]]}_{stringTables[1][buffer[0x18]].Replace('/', '-')}"; // hack
                        break;
                    case "TIRE":
                        outputName += $"_{stringTables[0][buffer[0xC]]}_{stringTables[1][buffer[0x10]].Replace('/', '-')}"; // hack
                        break;
                    case "DISPLAC":
                        outputName += $"_{stringTables[0][buffer[0x18]]}_{stringTables[1][buffer[0x1C]].Replace('/', '-')}"; // hack
                        break;
                }
                using (FileStream output = new($"{directory}\\{outputName}.dat", FileMode.Create, FileAccess.Write))
                {
                    output.Write(buffer);
                }
                i++;

                // DISPLAC - 0x10 - car ID
                // LWEIGHT - 0x03 - car ID
                // EQUIP - 0x00 - car ID - 0x08 - RM ID? - 0x2A - tyre ID
                // FLYWHEL - 0x04 - car ID - 0x06 - stage - 0x07 - stage = 0x08 - price
                // RACING - 0x0E - part ID? - 0x10 - price - 0x14 - string ID 1 - 0x18 - string ID 2 - RM part
                // SPEC - 0x184 - car price
                // TIRE - 0x00 - compound? - 0x06 - different per type - 0x07 same as 06 except last two sets when FF - 0x08 - price - 0x0C - string ID 1 - 0x10 - string ID 2

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
                using (StreamWriter output = new($"{directory}\\strings{i}.txt", false, Encoding.UTF8))
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
            using (StreamWriter output = File.CreateText($"{directory}\\Colours.csv"))
            {
                output.WriteLine($"\"CarID\",\"ColourID\",\"ColourName\"");

                foreach (byte[] structure in structs)
                {
                    using (MemoryStream stream = new(structure))
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

            using (FileStream file = new(filename, FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("@(#)");
                file.WriteCharacters(fileType);
                file.Position = 0x0C;
                file.WriteUShort(0x10); // always 0x10?
                WriteData(fileType, file, path);
            }
        }

        private static List<byte[]> ImportStructs(string directory)
        {
            string[] files = Directory.EnumerateFiles(directory, "*.dat").ToArray();
            List<byte[]> structs = new(files.Length);
            foreach (string file in files)
            {
                using (FileStream input = new(file, FileMode.Open, FileAccess.Read))
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
            List<List<string>> stringTables = new(files.Length);
            foreach (string file in files)
            {
                using (StreamReader input = new(file, Encoding.UTF8))
                {
                    List<string> strings = new();
                    while (!input.EndOfStream)
                    {
                        strings.Add(input.ReadLine());
                    }
                    stringTables.Add(strings);
                }
            }
            return stringTables;
        }

        private static void WriteData(string fileType, Stream file, string path)
        {
            List<byte[]> structs;
            List<List<string>> stringTables;

            if (fileType == "COLOR")
            {
                structs = new List<byte[]>();
                stringTables = new List<List<string>>();
                ReadColourData(structs, stringTables, path);
            }
            else
            {
                structs = ImportStructs(path);
                stringTables = ImportStringTables(path);
            }

            WriteMainStructs(structs, file);
            WriteStringTables(stringTables, file);

            while (file.Position % 4 > 0)
            {
                file.WriteByte(0);
            }
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

        private static void ReadColourData(List<byte[]> structs, List<List<string>> stringTables, string directory)
        {
            Dictionary<ushort, List<(byte, string)>> colours = new();
            using (StreamReader input = new($"{directory}\\Colours.csv"))
            {
                input.ReadLine(); // skip header
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();
                    string[] parts = line.Trim('"').Split(new[] { "\",\"" }, StringSplitOptions.None);
                    if (parts.Length != 3)
                    {
                        throw new Exception($"Invalid CSV: {line}");
                    }
                    ushort carID = ushort.Parse(parts[0]);
                    byte colourID = byte.Parse(parts[1], NumberStyles.HexNumber);
                    string colourName = parts[2];

                    if (!colours.TryGetValue(carID, out List<(byte, string)> colourList))
                    {
                        colourList = new List<(byte, string)>();
                        colours.Add(carID, colourList);
                    }
                    colourList.Add((colourID, colourName));
                }
            }

            for (int i = 0; i < 16; i++)
            {
                stringTables.Add(new List<string>());
            }

            foreach (KeyValuePair<ushort, List<(byte, string)>> car in colours)
            {
                ushort carID = car.Key;
                List<(byte, string)> carColours = car.Value;

                using (MemoryStream file = new(54))
                {
                    file.WriteUShort(carID);
                    for (int i = 0; i < 16; i++)
                    {
                        if (i < carColours.Count)
                        {
                            file.WriteByte(carColours[i].Item1);
                        }
                        else
                        {
                            file.WriteByte(0);
                        }
                    }
                    file.WriteUShort(0);

                    for (ushort i = 0; i < 16; i++)
                    {
                        ushort stringID = 0;
                        string name = "";
                        if (i < carColours.Count)
                        {
                            name = carColours[i].Item2;
                        }
                        int index = stringTables[i].IndexOf(name);
                        if (index == -1)
                        {
                            stringID = (ushort)stringTables[i].Count;
                            stringTables[i].Add(name);
                        }
                        else
                        {
                            stringID = (ushort)index;
                        }
                        file.WriteUShort(stringID);
                        file.WriteUShort(i);
                    }
                    structs.Add(file.ToArray());
                }
            }
        }
    }
}