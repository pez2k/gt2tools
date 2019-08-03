﻿using System.IO;
using System.Text;
using StreamExtensions;

namespace GT1.SpecSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            Dump();
        }

        private static void Dump()
        {
            string filename = "_unknown0013.spec";

            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Position = 4; // skip header
                string type = file.ReadCharacters();

                string directory = $"{Path.GetFileNameWithoutExtension(filename)}_{Path.GetExtension(filename)}_{type}";
                Directory.CreateDirectory(directory);

                file.Position = 0x0C;
                ushort structType = file.ReadUShort();
                if (structType == 0x10)
                {
                    ushort structCount = file.ReadUShort();
                    file.ReadUInt(); // always zero?
                    uint structSize = file.ReadUInt();

                    for (int i = 0; i < structCount; i++)
                    {
                        var buffer = new byte[structSize];
                        file.Read(buffer);

                        string outputName = $"{i:D4}";
                        if (type == "SPEC") {
                            outputName += $"_{Encoding.ASCII.GetString(buffer, 0, 5)}";
                        }
                        using (var output = new FileStream($"{directory}\\{outputName}.dat", FileMode.Create, FileAccess.Write))
                        {
                            output.Write(buffer);
                        }
                    }
                }
            }
        }
    }
}
