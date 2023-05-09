using System;
using System.IO;
using System.Linq;
using System.Text;
using StreamExtensions;

namespace GT1.Zip
{
    using LZSS;

    class Program
    {
        private const string Header = "LZIP";
        private const uint Version = 1;
        private const string Extension = ".gtz";

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            CheckFile(args[0]);
        }

        private static void CheckFile(string filename)
        {
            byte[] header = Encoding.ASCII.GetBytes(Header);

            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] existingHeader = new byte[4];
                file.Read(existingHeader);
                if (existingHeader.SequenceEqual(header))
                {
                    Decompress(filename, file);
                }
                else
                {
                    file.Position = 0;
                    Compress(filename, file);
                }
            }
        }

        private static void Decompress(string filename, Stream file)
        {
            if (file.ReadUInt() != Version)
            {
                Console.WriteLine("Unknown GTZ version");
                return;
            }

            uint compressedSize = file.ReadUInt();
            if (file.Length != compressedSize + 16)
            {
                Console.WriteLine("Incorrect file size");
                return;
            }

            uint uncompressedSize = file.ReadUInt();

            using (var compressed = new MemoryStream())
            {
                file.CopyTo(compressed);
                compressed.Position = 0;
                using (var decompressed = new MemoryStream())
                {
                    LZSS.Decompress(compressed, decompressed);

                    if (decompressed.Length < uncompressedSize)
                    {
                        Console.WriteLine("Decompressed data too short");
                        return;
                    }

                    decompressed.Position = 0;
                    decompressed.SetLength(uncompressedSize);
                    filename = filename.EndsWith(Extension) ? filename.Replace(Extension, "") : $"decompressed_{filename}";
                    using (var output = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    {
                        decompressed.CopyTo(output);
                    }
                }
            }
        }

        private static void Compress(string filename, Stream file)
        {
            using (var output = new FileStream($"{filename}{Extension}", FileMode.Create, FileAccess.Write))
            {
                output.WriteCharacters(Header);
                output.WriteUInt(Version);
                output.Position += 4;
                output.WriteUInt((uint)file.Length);
                using (var ms = new MemoryStream())
                {
                    LZSS.Compress(file, ms);
                    ms.Position = 0;
                    ms.CopyTo(output);
                }
                output.Position = 8;
                output.WriteUInt((uint)(output.Length - 16));
            }
        }
    }
}