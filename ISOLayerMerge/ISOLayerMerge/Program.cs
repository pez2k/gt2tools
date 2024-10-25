using System;
using System.IO;

namespace ISOLayerMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: ISOLayerMerge firstLayer.iso secondLayer.iso mergedOutputFile.iso");
                return;
            }
            else if (!File.Exists(args[0]) || !File.Exists(args[1]))
            {
                Console.WriteLine("Could not open one of the input files.");
                return;
            }

            using (var output = new FileStream(args[2], FileMode.Create, FileAccess.Write))
            {
                using (var firstLayer = new FileStream(args[0], FileMode.Open, FileAccess.Read))
                {
                    firstLayer.CopyTo(output);
                }
                using (var secondLayer = new FileStream(args[1], FileMode.Open, FileAccess.Read))
                {
                    secondLayer.Position = 0x8000;
                    secondLayer.CopyTo(output);
                }
            }
        }
    }
}
