using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StreamExtensions;

namespace GT3.ModelSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Incorrect arguments.");
                return;
            }

            Split(args[0]);
        }

        private static void Split(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var indexes = new List<uint>();
                uint nextIndex = file.ReadUInt();
                if (nextIndex != 0)
                {
                    throw new Exception("Index of header is not zero.");
                }

                do
                {
                    indexes.Add(nextIndex);
                    nextIndex = file.ReadUInt();
                }
                while (nextIndex > 0 && indexes.Count <= 100);

                if (indexes.Count == 100)
                {
                    throw new Exception("100 indexes read - probably not a valid file.");
                }

                for (int i = 1; i < indexes.Count; i++)
                {
                    uint blockStart = indexes[i];
                    uint blockEnd = (i == indexes.Count - 1) ? (uint)file.Length : indexes[i + 1];

                    if (blockEnd < blockStart)
                    {
                        throw new Exception("Invalid block size.");
                    }

                    uint blockSize = blockEnd - blockStart;
                    file.Position = blockStart;
                    byte[] buffer = new byte[blockSize];
                    file.Read(buffer);

                    string outputDirectory = Path.GetFileNameWithoutExtension(filename);
                    Directory.CreateDirectory(outputDirectory);
                    using (var output = new FileStream(Path.Combine(outputDirectory, $"{i:D3}.{GetOutputFileExtension(buffer.Take(4).ToArray())}"), FileMode.Create, FileAccess.Write))
                    {
                        output.Write(buffer);
                    }
                }
            }
        }

        private static string GetOutputFileExtension(byte[] header) =>
            Encoding.ASCII.GetString(header) switch
            {
                "GTM1" => "gtm1",
                "GTCI" => "gtci",
                "GTTR" => "gttr",
                "GTTW" => "gttw",
                "Vls0" => "vls0",
                "dumm" => "dummy",
                _ => "dat",
            };
    }
}
