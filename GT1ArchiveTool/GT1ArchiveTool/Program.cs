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
        
        private static readonly int[] alignmentModes = new int[] { 0, 4, 0x800 }; // some ARCs are unaligned, some aligned to 0x4, some aligned to 0x800

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string path = args[0];
                if ((File.GetAttributes(path) & FileAttributes.Directory) != 0)
                {
                    Rebuild(path, alignmentModes[0]);
                }
                else
                {
                    Extract(path);
                }
            }
            else if (args.Length == 2)
            {
                int.TryParse(args[0], out int alignmentMode);
                string path = args[1];
                if ((File.GetAttributes(path) & FileAttributes.Directory) != 0)
                {
                    Rebuild(path, alignmentModes[alignmentMode]);
                }
            }
            else
            {
                Console.WriteLine("Usage:\r\nGT1ArchiveTool.exe <ARC to unpack>\r\nGT1ArchiveTool.exe <directory to pack>\r\nGT1ArchiveTool.exe <alignment mode 0-2> <directory to pack>");
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

        private static void Rebuild(string path, int alignment)
        {
            using (var output = new FileStream($"{Path.GetFileName(path)}.DAT", FileMode.Create, FileAccess.Write))
            {
                output.WriteCharacters("@(#)GT-ARC");
                output.WriteUShort(0);
                output.WriteByte(1); // version?
                output.WriteByte(0); // compression flag, filled in once we've inspected the files to pack
                string[] files = Directory.EnumerateFiles(path).ToArray();
                output.WriteUShort((ushort)files.Length);
                output.SetLength(output.Position + (files.Length * 3 * 4));

                bool containsCompressedFiles = false;
                foreach (string filename in files)
                {
                    if (Path.GetExtension(filename) == Extension)
                    {
                        containsCompressedFiles = true;
                        ImportGTZip(filename, output, alignment);
                    }
                    else
                    {
                        ImportFile(filename, output, alignment);
                    }
                }

                if (alignment > 0)
                {
                    long paddedLength = output.Length;
                    while (paddedLength % alignment != 0)
                    {
                        paddedLength++;
                    }
                    output.SetLength(paddedLength);
                }

                if (containsCompressedFiles)
                {
                    output.Position = 0xD;
                    output.WriteByte(0x80);
                }
            }
        }

        private static void ImportGTZip(string filename, Stream output, int alignment)
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
                ImportData(file, output, compressedSize, uncompressedSize, alignment);
            }
        }

        private static void ImportFile(string filename, Stream output, int alignment)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                uint size = (uint)file.Length;
                ImportData(file, output, size, size, alignment);
            }
        }

        private static void ImportData(Stream input, Stream output, uint compressedSize, uint uncompressedSize, int alignment)
        {
            long offset = output.Length;
            if (alignment > 0)
            {
                while (offset % alignment != 0)
                {
                    offset++;
                }
            }
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
