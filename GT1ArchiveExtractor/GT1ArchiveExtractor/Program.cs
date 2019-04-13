using System;
using System.IO;

namespace GT1.ArchiveExtractor
{
    using LZSS;

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
                            LZSS.Decompress(compressed, decompressed);
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
    }
}