using System;
using System.IO;
using System.Linq;
using System.Text;
using StreamExtensions;

namespace GT3.PMBDumper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1 && Path.GetExtension(args[0]) == ".pmb")
            {
                Dump(args[0]);
            }
        }

        private static void Dump(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Position = 8;
                uint fileCount = file.ReadUInt();
                uint fileListStart = file.ReadUInt();

                file.Position = fileListStart;

                string directory = Path.GetFileNameWithoutExtension(filename);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                for (int i = 0; i < fileCount; i++)
                {
                    Console.WriteLine($"Extracting file {i} of {fileCount}...");
                    uint uncompressedSize = file.ReadUInt();
                    uint offset = file.ReadUInt();
                    long indexPosition = file.Position;
                    file.Position += 4;
                    uint nextOffset = i == fileCount - 1 ? (uint)file.Length : file.ReadUInt();

                    file.Position = offset;
                    byte[] header = new byte[4];
                    file.Read(header);

                    bool isGzip = false;
                    bool isTex1 = false;

                    if (header.Take(2).SequenceEqual(new byte[] { 0x1F, 0x8B }))
                    {
                        isGzip = true;
                    }
                    else if (header.SequenceEqual(Encoding.ASCII.GetBytes("Tex1")))
                    {
                        isTex1 = true;
                    }

                    uint endOfFile = nextOffset;
                    if (isGzip)
                    {
                        file.Position = nextOffset - 4;
                        while (file.ReadUInt() != uncompressedSize)
                        {
                            Console.WriteLine("End of gzip padded - rewinding one byte");
                            file.Position -= 5;

                            if (file.Position <= offset)
                            {
                                throw new Exception($"File {i} missing gzip end");
                            }
                        }
                        endOfFile = (uint)file.Position;
                    }

                    uint fileLength = endOfFile - offset;
                    file.Position = offset;
                    byte[] buffer = new byte[fileLength];
                    file.Read(buffer);

                    string extension = isGzip ? "gz" : isTex1 ? "img" : "dat";
                    string outfile = $"{i:D4}.{extension}";

                    using (var output = new FileStream(Path.Combine(directory, outfile), FileMode.Create, FileAccess.ReadWrite))
                    {
                        Console.WriteLine($"Writing {outfile}...");
                        output.Write(buffer);
                        if (isGzip)
                        {
                            output.Position -= 4;
                            uint gzipUncompressedSize = output.ReadUInt();

                            if (gzipUncompressedSize != uncompressedSize)
                            {
                                throw new Exception($"File {i} has uncompressed size of {uncompressedSize:X8} that doesn't match gzip size of {gzipUncompressedSize:X8}");
                            }
                        }
                    }

                    file.Position = indexPosition;
                }
            }
        }
    }
}
