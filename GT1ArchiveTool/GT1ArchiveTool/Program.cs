using System.IO;
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
                Extract(args[0]);
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
    }
}
