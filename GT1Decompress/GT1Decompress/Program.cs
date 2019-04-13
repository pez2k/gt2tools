using System.IO;

namespace GT1.Decompress
{
    using LZSS;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            using (FileStream input = new FileStream(args[0], FileMode.Open))
            {
                using (FileStream output = new FileStream(args[0] + "_decompressed", FileMode.Create))
                {
                    LZSS.Decompress(input, output);
                }
            }
        }
    }
}
