using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ICSharpCode.SharpZipLib.GZip;

namespace GT2.MenuSplitter
{
    using StreamExtensions;

    class Program
    {
        static bool decompress = true;
        static bool bruteforce = false;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Pack();
                return;
            }

            string filename;
            if (args.Length == 1)
            {
                filename = args[0];
            }
            else
            {
                if (args[0] == "-c" || args[1] == "-c")
                {
                    decompress = false;
                }
                if (args[0] == "-b" || args[1] == "-b")
                {
                    bruteforce = true;
                }
                filename = args.Last();
            }

            if (filename.Contains("gtmenudat."))
            {
                if (bruteforce)
                {
                    BruteForceExtract();
                }
                else
                {
                    Extract();
                }
            }
            else if (filename.Contains("commonpic."))
            {
                ExtractCommonpic();
            }
        }

        static void BruteForceExtract()
        {
            using (FileStream file = new FileStream("gtmenudat.dat", FileMode.Open, FileAccess.Read))
            {
                if (!Directory.Exists("gtmenudat"))
                {
                    Directory.CreateDirectory("gtmenudat");
                }

                long startPosition = 0;
                long nextPosition = 0;
                int fileNumber = 0;

                while (nextPosition < file.Length)
                {
                    nextPosition = FindNextGzip(file, startPosition + 1);

                    string filename = $"gt00{fileNumber:D4}.mdt{(decompress ? "" : ".gz")}";

                    Console.WriteLine($"File {filename} found from {startPosition} to {nextPosition}");

                    file.Position = startPosition;
                    int length = (int)(nextPosition - startPosition);
                    byte[] data = new byte[length];
                    file.Read(data, 0, length);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        stream.Write(data, 0, length);
                        stream.Position = 0;

                        using (FileStream output = new FileStream($"gtmenudat\\{filename}", FileMode.Create, FileAccess.Write))
                        {
                            if (decompress)
                            {
                                using (GZipStream unzip = new GZipStream(stream, CompressionMode.Decompress))
                                {
                                    unzip.CopyTo(output);
                                }
                            }
                            else
                            {
                                stream.CopyTo(output);
                            }
                        }
                    }

                    startPosition = nextPosition;
                    fileNumber++;
                }
                
            }
        }

        static long FindNextGzip(FileStream file, long startPosition)
        {
            for (long i = startPosition; i < file.Length; i++)
            {
                file.Position = i;

                if (file.ReadByte() == 0x1F)
                {
                    if (file.ReadByte() == 0x8B)
                    {
                        if (file.ReadByte() == 0x08)
                        {
                            if (file.ReadByte() == 0x00)
                            {
                                return i;
                            }
                        }
                    }
                }
            }

            return file.Length;
        }

        static void Extract()
        {
            using (FileStream file = new FileStream("gtmenudat.dat", FileMode.Open, FileAccess.Read))
            {
                using (FileStream index = new FileStream("gtmenudat.idx", FileMode.Open, FileAccess.Read))
                {
                    if (!Directory.Exists("gtmenudat"))
                    {
                        Directory.CreateDirectory("gtmenudat");
                    }

                    uint fileCount = index.ReadUInt();

                    for (int i = 0; i < fileCount; i++)
                    {
                        uint startPosition = index.ReadUInt();
                        uint padding = startPosition & 3;
                        startPosition -= padding;

                        uint endPosition;
                        endPosition = index.ReadUInt();
                        endPosition -= endPosition % 4;
                        index.Position -= 4;
                        endPosition -= padding;

                        file.Position = startPosition;
                        int length = (int)(endPosition - startPosition);
                        byte[] data = new byte[length];
                        file.Read(data, 0, length);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            stream.Write(data, 0, length);
                            stream.Position = 0;

                            string filename = $"gt00{i:D4}.mdt{(decompress ? "" : ".gz")}";
                            using (FileStream output = new FileStream($"gtmenudat\\{filename}", FileMode.Create, FileAccess.Write))
                            {
                                if (decompress)
                                {
                                    using (GZipStream unzip = new GZipStream(stream, CompressionMode.Decompress))
                                    {
                                        unzip.CopyTo(output);
                                    }
                                }
                                else
                                {
                                    stream.CopyTo(output);
                                }
                            }
                        }
                    }
                }
            }
        }

        static void Pack()
        {
            if (Directory.Exists("gtmenudat"))
            {
                PackGTMenu();
            }
            else if (Directory.Exists("commonpic"))
            {
                PackCommonpic();
            }
        }

        static void PackGTMenu()
        {
            using (var output = new FileStream("gtmenudat.dat", FileMode.Create, FileAccess.Write))
            {
                using (var index = new FileStream("gtmenudat.idx", FileMode.Create, FileAccess.Write))
                {
                    index.WriteUInt(0);
                    uint fileCount = 0;
                    long misalignedBytes = 0;

                    foreach (string filename in Directory.EnumerateFiles("gtmenudat\\"))
                    {
                        if (filename.EndsWith(".gz") && File.Exists(filename.Substring(0, filename.Length - 3)))
                        {
                            continue;
                        }

                        fileCount++;
                        long startPosition = output.Position;

                        Console.WriteLine($"Adding {filename}");
                        using (var input = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            if (filename.EndsWith(".gz"))
                            {
                                input.CopyTo(output);
                            }
                            else
                            {
                                using (var memory = new MemoryStream())
                                {
                                    using (var compression = new GZipOutputStream(memory))
                                    {
                                        compression.SetLevel(8);
                                        compression.IsStreamOwner = false;
                                        input.CopyTo(compression);
                                    }
                                    memory.Position = 0;
                                    memory.CopyTo(output);
                                }
                            }
                        }

                        misalignedBytes = output.Length % 4;
                        index.WriteUInt((uint)(startPosition + (misalignedBytes  == 0 ? 0 : (4 - misalignedBytes))));

                        if (misalignedBytes != 0)
                        {
                            output.Position += 4 - misalignedBytes;
                        }
                    }

                    output.SetLength(output.Position);
                    index.WriteUInt((uint)output.Length);
                    index.Position = 0;
                    index.WriteUInt(fileCount);
                }
            }
        }

        static void ExtractCommonpic()
        {
            using (FileStream index = new FileStream("commonpic.idx", FileMode.Open, FileAccess.Read))
            {
                uint fileCount = index.ReadUInt();
                uint startPosition = index.ReadUInt();
                uint nextPosition = index.ReadUInt();

                using (FileStream file = new FileStream("commonpic.dat", FileMode.Open, FileAccess.Read))
                {
                    if (!Directory.Exists("commonpic"))
                    {
                        Directory.CreateDirectory("commonpic");
                    }

                    for (int fileNumber = 0; fileNumber < fileCount; fileNumber++)
                    {
                        string filename = $"cmnp{fileNumber:D4}.dat";

                        Console.WriteLine($"File {filename} found from {startPosition} to {nextPosition}");

                        file.Position = startPosition;
                        byte[] data = new byte[nextPosition - startPosition];
                        file.Read(data);

                        using (FileStream output = new FileStream($"commonpic\\{filename}", FileMode.Create, FileAccess.Write))
                        {
                            output.Write(data);
                        }

                        startPosition = nextPosition;
                        if (index.Position == index.Length)
                        {
                            nextPosition = (uint)file.Length;
                        }
                        else
                        {
                            nextPosition = index.ReadUInt();
                        }
                    }
                }
            }
        }

        static void PackCommonpic()
        {
            using (var output = new FileStream("commonpic.dat", FileMode.Create, FileAccess.Write))
            {
                using (var index = new FileStream("commonpic.idx", FileMode.Create, FileAccess.Write))
                {
                    index.WriteUInt(0);
                    uint fileCount = 0;

                    foreach (string filename in Directory.EnumerateFiles("commonpic"))
                    {
                        fileCount++;
                        index.WriteUInt((uint)output.Position);

                        using (var input = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            Console.WriteLine($"Adding {filename}");
                            input.CopyTo(output);
                        }

                        long misalignedBytes = output.Length % 16;

                        if (misalignedBytes != 0)
                        {
                            output.Position += 16 - misalignedBytes;
                        }
                    }

                    index.WriteUInt((uint)output.Length);
                    index.Position = 0;
                    index.WriteUInt(fileCount);
                }
            }
        }
    }
}
