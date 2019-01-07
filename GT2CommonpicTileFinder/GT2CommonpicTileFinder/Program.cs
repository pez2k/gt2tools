using System;
using System.Collections.Generic;
using System.IO;

namespace GT2.CommonpicTileFinder
{
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            int[] outTiles = new int[] { 0x027B, 0x02B4, 0x04B6, 0x0419, 0x06D3, 0x0126, 0x02C5, 0x05A4, 0x03F4, 0x020A, 0x049D, 0x03C2, 0x0675, 0x03BC, 0x0575, 0x04D7, 0x0380, 0x0439, 0x021E, 0x04A0, 0x0171, 0x0247, 0x027F, 0x016B, 0x053A, 0x00F8, 0x029A, 0x015E, 0x036F, 0x0461, 0x0555, 0x0721, 0x0487, 0x046D, 0x01E9, 0x02E6, 0x030E, 0x034F, 0x04E0, 0x0338, 0x029C, 0x0166, 0x02DF, 0x022B, 0x0367, 0x0744 };
            int[] inTiles = new int[] { 0xA265, 0xD1A5, 0xE4D5, 0xA3C9, 0xF6E7, 0xA0D7, 0xF29A, 0x91AD, 0x101F, 0x4079, 0xA155, 0x916D, 0xA2CC, 0x4062, 0xA0B2, 0x90E9, 0x90FB, 0xC34E, 0xD1E1, 0x3045, 0x1019, 0x0007, 0x1023, 0x203C, 0x30BC, 0x60FD, 0x62B7, 0x6168, 0xD34F, 0xC353, 0xD59D, 0x102E, 0xF2F8, 0x214B, 0xF0F5, 0x000F, 0x3049, 0x0014, 0x305B, 0xB218, 0xB1DB, 0xB157, 0x6104, 0xA154, 0xA37F, 0xF707 };

            Copy("cmnp0312.dat", "cmnp0311.dat", inTiles, outTiles);
        }

        static void Translate()
        {
            int[] tileNumbers = new int[] { 0x027B, 0xA265, 0x02B4, 0xD1A5, 0x04B6, 0x0419, 0xE4D5, 0xA3C9, 0x06D3, 0x0126, 0xF6E7, 0xA0D7, 0x02C5, 0xF707, 0xF29A, 0x05A4, 0x03F4, 0x91AD, 0x101F, 0x049D, 0x0155, 0x03C2, 0x0675, 0x016D, 0xA2CC, 0x03BC, 0x0575, 0x4062, 0xA0B2, 0x04D7, 0x0380, 0x90E9, 0x90FB, 0x0439, 0x021E, 0xC34E, 0xD1E1, 0x04A0, 0x0171, 0x3045, 0x1019, 0x0247, 0x027F, 0x0007, 0x1023, 0x016B, 0x053A, 0x203C, 0x30BC, 0x00F8, 0x029A, 0x60FD, 0x62B7, 0x015E, 0x036F, 0x6168, 0xD34F, 0x0461, 0x0555, 0xC353, 0xD59D, 0x0721, 0x0487, 0x102E, 0xF2F8 };
            foreach (int tile in tileNumbers)
            {
                Console.WriteLine($"{tile:X4} = 0x{GetPosition(tile):X5}");
            }
            Console.ReadKey();
        }

        static int GetPosition(int tile)
        {
            int t = tile & 0xFFF;
            return ((t / 32) * 4096) + ((t % 32) * 16) + 0x4000;
        }

        static void Copy(string infilename, string outfilename, int[] inTiles, int[] outTiles)
        {
            using (var infile = new FileStream(infilename, FileMode.Open, FileAccess.Read))
            {
                using (var outfile = new FileStream(outfilename, FileMode.Open, FileAccess.ReadWrite))
                {
                    infile.Position = 0x1F84 + 0x200;
                    List<ushort> inpalette = new List<ushort>(256);
                    outfile.Position = 0x1F84 + 0x200;
                    List<ushort> outpalette = new List<ushort>(256);

                    for (int i = 0; i < 256; i++)
                    {
                        inpalette.Add(infile.ReadUShort());
                        outpalette.Add(outfile.ReadUShort());
                    }

                    for (int i = 0; i < inTiles.Length; i++)
                    {
                        infile.Position = GetPosition(inTiles[i]);
                        outfile.Position = GetPosition(outTiles[i]);

                        byte[] buffer = new byte[16];
                        infile.Read(buffer);
                        MapPalette(inTiles[i], buffer, inpalette, outpalette);
                        outfile.Write(buffer);
                        for (int j = 1; j < 8; j++)
                        {
                            infile.Position += 512 - 16;
                            outfile.Position += 512 - 16;
                            infile.Read(buffer);
                            MapPalette(inTiles[i], buffer, inpalette, outpalette);
                            outfile.Write(buffer);
                        }
                    }
                }
            }
        }

        static void MapPalette(int intile, byte[] buffer, List<ushort> inpalette, List<ushort> outpalette)
        {
            if ((intile & 0xF000) == 0x1000)
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    byte id = buffer[i];
                    ushort colour = inpalette[id];
                    int newid = outpalette.FindIndex(x => x == colour);
                    if (newid >= 0)
                    {
                        buffer[i] = (byte)newid;
                        break;
                    }
                }
            }
        }
    }
}
