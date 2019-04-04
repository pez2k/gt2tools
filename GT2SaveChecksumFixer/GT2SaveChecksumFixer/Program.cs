using System;
using System.IO;
using Force.Crc32;

namespace GT2.SaveFixer
{
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No filename provided.");
                return;
            }

            string filename = args[0];

            using (var save = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                Console.WriteLine($"Save: {filename}");
                const int start = 0x80;
                const int end = 0x7F1C;
                save.Position = start;
                byte[] buffer = new byte[end - start];
                save.Read(buffer);
                uint checksum = Crc32Algorithm.Compute(buffer);
                Console.WriteLine($"Checksum: 0x{checksum:X4}");
                save.WriteUInt(checksum);
            }
        }
    }
}
