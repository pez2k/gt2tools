using System;
using System.IO;
using System.IO.Compression;
using StreamExtensions;

namespace PolyphonyPS2Zip
{
    class Program
    {
        private const string Extension = ".ps2zip";
        private const uint Magic = 0xFFF7EEC5;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments.");
                return;
            }

            CheckFile(args[0]);
        }

        static void CheckFile(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                string filename = Path.GetFileName(filePath);
                if (file.ReadUInt() == Magic)
                {
                    Extract(file, filePath);
                }
                else
                {
                    if (Path.GetExtension(filePath) == Extension)
                    {
                        Console.WriteLine("Not a valid PS2Zip file.");
                        return;
                    }
                    file.Position = 0;
                    Compress(file, filePath);
                }
            }
        }

        static void Extract(Stream file, string filePath)
        {
            int uncompressedSize = -file.ReadInt();

            string outputFile = Path.GetExtension(filePath) == Extension ? filePath.Substring(0, filePath.Length - Extension.Length) : filePath + "_decompressed";
            using (var output = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
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

        static void Compress(Stream file, string filePath)
        {
            using (var output = new FileStream(filePath + Extension, FileMode.Create, FileAccess.Write))
            {
                output.WriteUInt(Magic);
                output.WriteInt(-(int)file.Length);

                using (var compression = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    file.CopyTo(compression);
                }
            }
        }
    }
}
