using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2MenuSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists("gtmenudat"))
            {
                Directory.CreateDirectory("gtmenudat");
            }

            using (FileStream file = new FileStream("gtmenudat.dat", FileMode.Open, FileAccess.Read))
            {
                long startPosition = 0;
                long nextPosition = 0;
                int fileNumber = 0;

                while (nextPosition < file.Length)
                {
                    nextPosition = FindNextGzip(file, startPosition + 1);

                    Console.WriteLine($"File {fileNumber} found from {startPosition} to {nextPosition}");

                    file.Position = startPosition;
                    int length = (int)(nextPosition - startPosition);
                    byte[] data = new byte[length];
                    file.Read(data, 0, length);

                    /*using (FileStream output = new FileStream($"gtmenudat\\{fileNumber}.dat", FileMode.Create, FileAccess.Write))
                    {
                        using (GZipStream zip = new GZipStream(output, CompressionMode.Decompress))
                        {
                            zip.Write(data, 0, length);
                        }
                    }*/

                    using (MemoryStream stream = new MemoryStream())
                    {
                        stream.Write(data, 0, length);
                        stream.Position = 0;

                        using (FileStream output = new FileStream($"gtmenudat\\{fileNumber:X4}.dat", FileMode.Create, FileAccess.Write))
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
