using System.IO;

namespace GT1.Compress
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
                using (FileStream output = new FileStream(args[0] + "_compressed", FileMode.Create))
                {
                    LZSS.Compress(input, output);
                }
            }
        }
    }
}
