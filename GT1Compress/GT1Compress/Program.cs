using System;
using System.IO;

namespace GT1.Compress
{
    using LZSS;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length > 2)
            {
                Console.WriteLine("Usage:\r\nGT1Compress <filename>\r\nOR\r\nGT1Compress <compression level 1 - 32> <filename>\r\n\r\ne.g.: GT1Compress 4 tsplr.tex");
                return;
            }

            string filename;
            int windowSize = 1024;
            if (args.Length == 2)
            {
                if (int.TryParse(args[0], out int compressionLevel) && compressionLevel >= 0 && compressionLevel <= 32)
                {
                    windowSize = compressionLevel * 1024;
                }
                filename = args[1];
            }
            else
            {
                filename = args[0];
            }

            using (FileStream input = new FileStream(filename, FileMode.Open))
            {
                using (FileStream output = new FileStream($"{filename}_compressed", FileMode.Create))
                {
                    if (windowSize == 0)
                    {
                        LZSS.FakeCompress(input, output);
                    }
                    else
                    {
                        LZSS.Compress(input, output, windowSize);
                    }
                }
            }
        }
    }
}
