using System;
using System.IO;
using System.Text;
using StreamExtensions;

namespace GT3.GTARExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No file specified.");
                return;
            }

            string filename = args[0];

            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] magic = new byte[4];
                file.Read(magic);
                if (Encoding.ASCII.GetString(magic) != "GTAR")
                {
                    Console.WriteLine("Not a GTAR archive.");
                    return;
                }

                string directory = $"{Path.GetFileNameWithoutExtension(filename)}_extracted";
                Directory.CreateDirectory(directory);

                uint fileCount = file.ReadUInt();
                uint dataStart = file.ReadUInt();
                uint unknown = file.ReadUInt();

                for (int i = 0; i < fileCount; i++)
                {
                    file.Position = (i * 4) + 0x10;
                    uint start = file.ReadUInt();
                    uint end = file.ReadUInt();
                    uint length = end - start;

                    file.Position = start + dataStart;
                    byte[] buffer = new byte[length];
                    file.Read(buffer);

                    using (var outFile = new FileStream($"{directory}\\{i:D2}.dat", FileMode.Create, FileAccess.Write))
                    {
                        outFile.Write(buffer);
                    }
                }
            }
        }
    }
}