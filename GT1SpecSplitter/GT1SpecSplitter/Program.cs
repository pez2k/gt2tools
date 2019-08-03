using System;
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
                DumpMainStruct(file, fileType, directory);
                if (fileType == "SPEC" || fileType == "COLOR")
                {
                    DumpStringTables(file, directory);
                }
            }
        }

        private static void DumpMainStruct(Stream file, string fileType, string directory)
        {
            ushort structCount = file.ReadUShort();
            file.ReadUInt(); // always zero?
            uint structSize = file.ReadUInt();

            for (int i = 0; i < structCount; i++)
            {
                var buffer = new byte[structSize];
                file.Read(buffer);

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
            }
        }

        private static void DumpStringTables(Stream file, string directory)
        {
            ushort tableCount = file.ReadUShort();
            if (tableCount == 2)
            {
                file.Position += 10;
            }
            else if (tableCount == 16)
            {
                file.Position += 66;
            }

            for (int i = 0; i < tableCount; i++)
            {
                DumpStrings(file, directory, $"strings{i}");
            }
        }

        private static void DumpStrings(Stream file, string directory, string filename)
        {
            ushort stringCount = file.ReadUShort();
            using (var output = new StreamWriter($"{directory}\\{filename}.txt"))
            {
                for (int i = 0; i < stringCount; i++)
                {
                    byte stringLength = file.ReadSingleByte();
                    var buffer = new byte[stringLength];
                    file.Read(buffer);
                    string textString = Encoding.ASCII.GetString(buffer);
                    output.WriteLine(textString);
                    file.Position++;
                }
                output.Flush();
            }
            if (file.Position % 2 > 0)
            {
                file.Position++;
            }
        }
    }
}
