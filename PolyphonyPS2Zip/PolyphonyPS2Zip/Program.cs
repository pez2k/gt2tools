using System;
using System.IO;
using System.IO.Compression;
using StreamExtensions;

namespace PolyphonyPS2Zip
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments.");
                return;
            }

            Extract(args[0]);
        }

        static void Extract(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                if (file.ReadUInt() != 0xFFF7EEC5)
                {
                    Console.WriteLine("Not a valid PS2Zip file.");
                    return;
                }
                int uncompressedSize = -file.ReadInt();

                using (var output = new FileStream($"decompressed_{Path.GetFileName(filename)}", FileMode.Create, FileAccess.Write))
                {
                    using (var decompression = new DeflateStream(file, CompressionMode.Decompress))
                    {
                        decompression.CopyTo(output);
                    }
                    if (output.Position != uncompressedSize)
                    {
                        Console.WriteLine($"Warning: file size {output.Position} does not match expected size of {uncompressedSize}.");
                    }
                }
            }
        }
    }
}
