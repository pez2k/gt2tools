﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class Palette
    {
        private const byte ColourCount = 16;

        private readonly ushort[] colours = new ushort[ColourCount];

        public void LoadFromGameFile(Stream file)
        {
            for (int i = 0; i < ColourCount; i++)
            {
                colours[i] = file.ReadUShort();
            }
        }

        public void WriteToBitmapPalette(ColorPalette palette)
        {
            for (int i = 0; i < ColourCount; i++)
            {
                ushort paletteColour = colours[i];
                int R = paletteColour & 0x1F;
                int G = (paletteColour >> 5) & 0x1F;
                int B = (paletteColour >> 10) & 0x1F;

                palette.Entries[i] = Color.FromArgb(R * 8, G * 8, B * 8);
            }
        }

        public void WriteToJASCPalette(Stream file)
        {
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine("JASC-PAL");
                writer.WriteLine("0100");
                writer.WriteLine("16");
                foreach (ushort colour in colours)
                {
                    int R = colour & 0x1F;
                    int G = (colour >> 5) & 0x1F;
                    int B = (colour >> 10) & 0x1F;
                    writer.WriteLine($"{R * 8} {G * 8} {B * 8}");
                }
            }
        }
    }
}
