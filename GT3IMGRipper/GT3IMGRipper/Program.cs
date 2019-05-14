using System;
using System.IO;
using StreamExtensions;

namespace GT3IMGRipper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                RipIMGs(args[0]);
            }
        }

        private static void RipIMGs(string filename)
        {
            string directory = Path.GetFileNameWithoutExtension(filename);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            int i = 0;
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                while (file.Position < (file.Length - 16))
                {
                    if (file.ReadSingleByte() == 'T')
                    {
                        if (file.ReadSingleByte() == 'e' && file.ReadSingleByte() == 'x' && file.ReadSingleByte() == '1')
                        {
                            long fileStart = file.Position - 4;
                            Console.WriteLine($"Found Tex1 at {fileStart:X8}");
                            file.Position += 8;
                            uint fileSize = file.ReadUInt();

                            byte[] buffer = new byte[fileSize];
                            file.Position = fileStart;
                            file.Read(buffer);

                            string outfile = $"{i++:D4}.img";
                            using (var output = new FileStream(Path.Combine(directory, outfile), FileMode.Create, FileAccess.Write))
                            {
                                Console.WriteLine($"Writing {outfile}");
                                output.Write(buffer);
                            }
                        }
                        file.Position -= 1;
                    }
                }
            }
        }
    }
}
