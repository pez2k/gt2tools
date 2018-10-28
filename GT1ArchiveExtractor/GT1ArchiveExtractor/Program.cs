using System;
using System.IO;

namespace GT2.GT1ArchiveExtractor
{
    using StreamExtensions;

    class Program
    {
        public static void Main(string[] args)
        {
            ExtractFiles(new DirectoryFileList("./"));
        }

        private static void ExtractFiles(FileList fileList)
        {
            Console.WriteLine($"Extracting {fileList.Name}");

            if (!Directory.Exists(fileList.Name))
            {
                Directory.CreateDirectory(fileList.Name);
            }

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
                    ExtractFiles(new ArchiveFileList(Path.Combine(fileList.Name, file.Name), file.Contents));
                }
                else
                {
                    using (FileStream output = File.OpenWrite(Path.Combine(fileList.Name, $"{file.Name}.{file.GetExtension()}")))
                    {
                        output.Write(file.Contents);
                    }
                }

            }
        }

        private static void Decompress(Stream compressed, Stream output)
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