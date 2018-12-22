using System;
using System.IO;
using System.IO.Compression;

namespace GT2MenuSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            Extract();
        }

        static void Extract()
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

                    string filename = $"gt00{fileNumber:D4}.mdt";

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
                            using (GZipStream unzip = new GZipStream(stream, CompressionMode.Decompress))
                            {
                                unzip.CopyTo(output);
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
    }
}
