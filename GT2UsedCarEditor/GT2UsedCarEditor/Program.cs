using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace GT2UsedCarEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                FileAttributes attributes = File.GetAttributes(args[0]);

                if (attributes.HasFlag(FileAttributes.Directory))
                {
                    if (Directory.Exists(args[0]))
                    {
                        WriteFile(args[0]);
                    }
                }
                else if (File.Exists(args[0]))
                {
                    ReadFile(args[0]);
                }
            }
        }

        static void ReadFile(string filename)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        using (GZipStream unzip = new GZipStream(file, CompressionMode.Decompress))
                        {
                            unzip.CopyTo(stream);
                        }
                    }
                    var list = new UsedCarList();
                    list.Read(stream);
                    list.WriteCSV(filename.Replace(".", ""));
                }
            }
            catch
            {
                Console.WriteLine($"Unable to load file {filename}.");
            }
        }

        static void WriteFile(string directory)
        {
            var list = new UsedCarList();
            list.ReadCSV(directory);

            using (MemoryStream stream = new MemoryStream())
            {
                list.Write(stream);
                stream.Position = 0;
                
                using (FileStream file = new FileStream("." + directory.Split('\\').Last(), FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new GZipStream(file, CompressionMode.Compress))
                    {
                        stream.CopyTo(zip);
                    }
                }
            }
        }
    }
}
