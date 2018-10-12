using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace GT2.GT1ArchiveExtractor
{
    using StreamExtensions;

    class Program
    {
        static readonly byte[] ARCHeader = new byte[11] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x41, 0x52, 0x43, 0x00 };
        static readonly byte[] TEXHeader = new byte[11] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x43, 0x54, 0x45, 0x58 };
        static readonly byte[] CARHeader = new byte[11] { 0x40, 0x28, 0x23, 0x29, 0x47, 0x54, 0x2D, 0x43, 0x41, 0x52, 0x00 };

        static void Main(string[] args)
        {
            ProcessDirectory("./");
        }
        
        static void ProcessDirectory(string path)
        {
            Console.WriteLine($"Scanning {path}");
            var directoryInfo = new DirectoryInfo(path);
            IEnumerable<FileInfo> files = directoryInfo.EnumerateFiles();
            foreach(FileInfo file in files)
            {
                byte[] header = new byte[11];
                using (FileStream stream = file.OpenRead())
                {
                    stream.Read(header, 0, 11);
                }

                if (header.SequenceEqual(TEXHeader))
                {
                    Console.WriteLine($"Renaming TEX file {file.Name}");
                    file.MoveTo(file.FullName.Replace(".dat", ".tex"));
                }
                else if (header.SequenceEqual(CARHeader))
                {
                    Console.WriteLine($"Renaming CAR file {file.Name}");
                    file.MoveTo(file.FullName.Replace(".dat", ".car"));
                }

                // try decompressing

                if (header.SequenceEqual(ARCHeader))
                {
                    Console.WriteLine($"Extracting {file.Name}");
                    
                    string directory = path + "\\" + file.Name.Replace(file.Extension, "");

                    using (FileStream stream = file.OpenRead())
                    {
                        ExtractARC(directory, stream);
                    }

                    ProcessDirectory(directory);
                }
            }

            // foreach file in directory, sniff header and try adding extensions
            // foreach file in directory, check for names in filelist.txt
            // foreach arc in directory, extract and recurse into extracted directory
        }

        static void ExtractARC(string fileName, Stream file)
        {
            file.Position = 0x0E;
            ushort fileCount = file.ReadUShort();

            Directory.CreateDirectory(fileName);

            for (ushort i = 0; i < fileCount; i++)
            {
                uint offset = file.ReadUInt();
                uint size = file.ReadUInt();
                uint uncompressedSize = file.ReadUInt();

                long indexPosition = file.Position;

                file.Position = offset;
                using (FileStream output = new FileStream(CreateOutputFilename(fileName), FileMode.Create, FileAccess.ReadWrite))
                {
                    byte[] buffer = new byte[size];
                    file.Read(buffer);
                    
                    if (size != uncompressedSize)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                        {
                            Decompress(memoryStream, output);
                        }
                    }
                    else
                    {
                        output.Write(buffer);
                    }
                }
                file.Position = indexPosition;
            }
        }

        static string CreateOutputFilename(string directoryName)
        {
            string number = Directory.GetFiles(directoryName).Length.ToString();

            for (int i = number.Length; i < 4; i++)
            {
                number = "0" + number;
            }

            return directoryName + "\\_unknown" + number + ".dat";
        }

        static void Decompress(Stream compressed, Stream output)
        {
            byte ch;
            byte Temp;
            byte Temp1;
            byte Temp2;
            int i;
            int jump;
            int size;
            int flag;
            byte[] b = new byte[258];
            long Temp_Offset;
            long Last_Offset;

            flag = 1;
            compressed.Position = 0;

            while (compressed.Position < compressed.Length)
            {
                if (flag == 1)
                {
                    Temp = (byte)compressed.ReadByte();
                    flag = Temp | 0x100;
                }
                ch = (byte)compressed.ReadByte();

                if ((flag & 1) != 1)
                {
                    output.WriteByte(ch);
                }
                else
                {
                    Temp1 = (byte)compressed.ReadByte();
                    if ((Temp1 & 0x80) != 0x80)
                    {
                        jump = (Temp1 & 0x7F) + 1;
                    }
                    else
                    {
                        Temp2 = (byte)compressed.ReadByte();
                        jump = ((Temp1 & 0x7F) * 256) + Temp2 + 1;
                    }
                    size = ch + 3;
                    Temp_Offset = output.Position;
                    output.Position = output.Position - jump;
                    Last_Offset = output.Position;
                    for (i = 0; i < size; i++)
                    {
                        b[i] = (byte)output.ReadByte();
                        if (output.Position >= output.Length)
                        {
                            output.Position = Last_Offset;
                        }
                    }
                    output.Position = Temp_Offset;
                    output.Write(b, 0, size);
                }
                flag = flag >> 1;
            }
        }
    }
}