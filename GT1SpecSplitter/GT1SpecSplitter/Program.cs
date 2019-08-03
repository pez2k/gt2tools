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
                while (file.Position < file.Length)
                {
                    ushort structType = file.ReadUShort();
                    switch (structType)
                    {
                        case 0x10:
                            DumpMainStruct(file, fileType, directory);
                            break;
                        case 0x02:
                            DumpCarNameStrings(file, directory);
                            break;
                        case 0x00:
                            DumpColourNameStrings(file, directory);
                            break;
                        default:
                            Console.WriteLine($"Unknown struct type 0x{structType:X4}.");
                            return;
                    }
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

        private static void DumpCarNameStrings(Stream file, string directory)
        {
            file.Position += 0x0A;
            DumpStrings(file, directory, "strings1");
            file.Position++;
            DumpStrings(file, directory, "strings2");
            file.Position++;
        }

        private static void DumpColourNameStrings(Stream file, string directory)
        {
            file.Position += 0x36;
            DumpStrings(file, directory, "strings1");
            DumpStrings(file, directory, "strings2");
            file.Position++;
            DumpStrings(file, directory, "strings3");
            DumpStrings(file, directory, "strings4");
            file.Position++;
            DumpStrings(file, directory, "strings5");
            DumpStrings(file, directory, "strings6");
            DumpStrings(file, directory, "strings7");
            file.Position++;
            DumpStrings(file, directory, "strings8");
            DumpStrings(file, directory, "strings9");
            file.Position++;
            DumpStrings(file, directory, "strings10");
            file.Position++;
            DumpStrings(file, directory, "strings11");
            file.Position++;
            DumpStrings(file, directory, "strings12");
            DumpStrings(file, directory, "strings13");
            file.Position++;
            DumpStrings(file, directory, "strings14");
            file.Position++;
            DumpStrings(file, directory, "strings15");
            DumpStrings(file, directory, "strings16");
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
        }
    }
}
