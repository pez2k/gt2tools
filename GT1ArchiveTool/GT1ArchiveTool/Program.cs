using System;
using System.IO;
using System.Linq;
using System.Text;
using StreamExtensions;

namespace GT1ArchiveTool
{
    class Program
    {
        private const string Header = "LZIP";
        private const uint Version = 1;
        private const string Extension = ".gtz";

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string path = args[0];
                if ((File.GetAttributes(path) & FileAttributes.Directory) != 0)
                {
                    Rebuild(path);
                }
                else
                {
                    Extract(path);
                }
            }
        }

        private static void Extract(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                string directory = Path.GetFileNameWithoutExtension(filename);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                stream.Position = 0x0E;
                ushort fileCount = stream.ReadUShort();

                for (ushort i = 0; i < fileCount; i++)
                {
                    uint offset = stream.ReadUInt();
                    uint size = stream.ReadUInt();
                    uint uncompressedSize = stream.ReadUInt();

                    long indexPosition = stream.Position;

                    stream.Position = offset;

                    byte[] buffer = new byte[size];
                    stream.Read(buffer);

                    string entryName = Path.Combine(directory, $"{i:D4}");

                    if (size != uncompressedSize)
                    {
                        CreateGTZip(entryName, size, uncompressedSize, buffer);
                    }
                    else
                    {
                        CreateFile(entryName, buffer);
                    }

                    stream.Position = indexPosition;
                }
            }
        }

        private static void CreateFile(string filename, byte[] contents)
        {
            using (var output = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                output.Write(contents);
            }
        }

        private static void CreateGTZip(string filename, uint compressedSize, uint uncompressedSize, byte[] contents)
        {
            using (var output = new FileStream($"{filename}{Extension}", FileMode.Create, FileAccess.Write))
            {
                output.WriteCharacters(Header);
                output.WriteUInt(Version);
                output.WriteUInt(compressedSize);
                output.WriteUInt(uncompressedSize);
                output.Write(contents);
            }
        }

        private static void Rebuild(string path)
        {
            using (var output = new FileStream($"{Path.GetFileName(path)}.dat", FileMode.Create, FileAccess.Write))
            {
                output.WriteCharacters("@(#)GT-ARC");
                output.WriteUShort(0);
                output.WriteUShort(0x8001); // 00 01 in CARINF.DAT, seems not to matter, but breaks CAR.DAT if 00 01
                string[] files = Directory.EnumerateFiles(path).ToArray();
                output.WriteUShort((ushort)files.Length);
                output.SetLength(output.Position + (files.Length * 3 * 4));

                foreach (string filename in files)
                {
                    if (Path.GetExtension(filename) == Extension)
                    {
                        ImportGTZip(filename, output);
                    }
                    else
                    {
                        ImportFile(filename, output);
                    }
                }
            }
        }

        private static void ImportGTZip(string filename, Stream output)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] existingHeader = new byte[4];
                file.Read(existingHeader);
                if (!existingHeader.SequenceEqual(Encoding.ASCII.GetBytes(Header)))
                {
                    throw new Exception("Not a GTZ");
                }

                if (file.ReadUInt() != Version)
                {
                    throw new Exception("Unknown GTZ version");
                }

                uint compressedSize = file.ReadUInt();
                if (file.Length != compressedSize + 16)
                {
                    throw new Exception("Incorrect file size");
                }

                uint uncompressedSize = file.ReadUInt();
                ImportData(file, output, compressedSize, uncompressedSize);
            }
        }

        private static void ImportFile(string filename, Stream output)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                uint size = (uint)file.Length;
                ImportData(file, output, size, size);
            }
        }

        private static void ImportData(Stream input, Stream output, uint compressedSize, uint uncompressedSize)
        {
            long offset = output.Length;
            output.WriteUInt((uint)offset);
            output.WriteUInt(compressedSize);
            output.WriteUInt(uncompressedSize);
            long indexPosition = output.Position;
            output.Position = offset;
            input.CopyTo(output);
            output.Position = indexPosition;
        }
    }
}
