using System;
using System.IO;

namespace GT2.GT1ArchiveExtractor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IFileWriter writer = new DiskFileWriter();

            if (args.Length == 1)
            {
                if (args[0] == "-l")
                {
                    writer = new FileListFileWriter();
                }
                else if (args[0].EndsWith(".txt"))
                {
                    writer = new DiskFileWriter(args[0]);
                }
            }

            ExtractFiles(new DirectoryFileList(".\\"), writer);
        }

        private static void ExtractFiles(FileList fileList, IFileWriter writer)
        {
            Console.WriteLine($"Extracting {fileList.Name}");
            
            writer.CreateDirectory(fileList.Name);

            foreach (FileData file in fileList.GetFiles())
            {
                if (file.Compressed)
                {
                    using (var compressed = new MemoryStream(file.Contents))
                    {
                        using (var decompressed = new MemoryStream())
                        {
                            Decompress(compressed, decompressed);
                            file.Contents = decompressed.GetBuffer();
                        }
                    }
                }

                if (file.IsArchive())
                {
                    ExtractFiles(new ArchiveFileList(Path.Combine(fileList.Name, file.Name), file.Contents), writer);
                }
                else
                {
                    writer.Write(Path.Combine(fileList.Name, $"{file.Name}.{file.GetExtension()}"), file.Contents);
                }
            }
        }

        private static void Decompress(Stream compressed, Stream output)
        {
            byte[] decompressedBytes = new byte[258];
            byte isCompressedFlag = 1;
            compressed.Position = 0;

            while (compressed.Position < compressed.Length)
            {
                if (isCompressedFlag == 1) // if flag is 1 (we've run out of bits), read new flag
                {
                    isCompressedFlag = (byte)(compressed.ReadByte() | 0x100);
                }

                if ((isCompressedFlag & 1) != 1) // not compressed, shorter than dictionary size? next byte is data
                {
                    byte uncompressedByte = (byte)compressed.ReadByte();
                    output.WriteByte(uncompressedByte);
                }
                else // compressed, next byte is length - 3 (3 being dict size)
                {
                    ushort distance;
                    ushort lengthOfDecompressedData = (ushort)(compressed.ReadByte() + 3);
                    byte distanceFirstByte = (byte)compressed.ReadByte(); // next byte(s) must be step back distance
                    if ((distanceFirstByte & 0x80) != 0x80) // 1 byte distance?
                    {
                        distance = (ushort)((distanceFirstByte & 0x7F) + 1);
                    }
                    else // 2 byte distance?
                    {
                        byte distanceSecondByte = (byte)compressed.ReadByte();
                        distance = (ushort)(((distanceFirstByte & 0x7F) * 256) + distanceSecondByte + 1);
                    }
                    long endOfOutput = output.Position; // store end of output position
                    output.Position = output.Position - distance; // step back the distance / offset - no bound, therefore infinitely sized window
                    long steppedBackPosition = output.Position;
                    for (int i = 0; i < lengthOfDecompressedData; i++) // read for the the length, looping around if length > distance
                    {
                        decompressedBytes[i] = (byte)output.ReadByte(); // up to 8 bit max + 3 (258)
                        if (output.Position >= output.Length)
                        {
                            output.Position = steppedBackPosition;
                        }
                    }
                    output.Position = endOfOutput; // write data at end of output
                    output.Write(decompressedBytes, 0, lengthOfDecompressedData);
                }
                isCompressedFlag = (byte)(isCompressedFlag >> 1); // bit handled, move on to the next one
            }
        }
    }
}