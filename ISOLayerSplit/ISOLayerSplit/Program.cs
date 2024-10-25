using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ISOLayerSplit
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ISOLayerSplit <input disc image> <output filename for second layer>");
                Console.WriteLine("e.g. ISOLayerSplit game.iso layer2.iso");
                return;
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("Could not open the input file.");
                return;
            }

            using (var disc = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            {
                bool foundFirstHeader = false;
                long secondHeaderPosition = 0;

                Console.WriteLine("Scanning disc...");

                while (disc.Position < disc.Length) // Scan the image for CD001 v1 headers - they'll only ever be aligned to the start of a block, with a block size of 0x8000
                {
                    long position = disc.Position;
                    byte[] buffer = new byte[8];
                    disc.Read(buffer, 0, buffer.Length);
                    if (buffer.SequenceEqual(new byte[] { 0x01, 0x43, 0x44, 0x30, 0x30, 0x31, 0x01, 0x00 })) // 0x01 "CD001" 0x01 0x00
                    {
                        byte[] characters = new byte[32];
                        disc.Read(characters, 0, characters.Length); // Read the system and volume identifiers as they're right there and are helpful for the end user to see
                        string system = Encoding.ASCII.GetString(characters).Trim();
                        disc.Read(characters, 0, characters.Length);
                        string volume = Encoding.ASCII.GetString(characters).Trim();

                        Console.WriteLine($"Found CD001 v1 header at 0x{position:X}\r\nSystem: {system}\r\nVolume: {volume}\r\n");

                        if (foundFirstHeader)
                        {
                            secondHeaderPosition = position;
                            break;
                        }
                        else
                        {
                            foundFirstHeader = true;
                        }
                    }
                    disc.Position = position + 0x8000;
                }

                if (secondHeaderPosition == 0)
                {
                    Console.WriteLine("Unable to find second layer header.");
                    return;
                }

                using (var secondLayer = new FileStream(args[1], FileMode.Create, FileAccess.Write))
                {
                    Console.WriteLine("Extracting second layer...");
                    secondLayer.Position = 0x8000;
                    disc.Position = secondHeaderPosition;
                    disc.CopyTo(secondLayer);
                }
            }
        }
    }
}