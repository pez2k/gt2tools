using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class Palette
    {
        private const byte ColourCount = 16;
        private const ushort AlphaBit = 0x8000;

        private readonly ushort[] colours = new ushort[ColourCount];

        public bool IsEmpty
        {
            get
            {
                foreach (ushort colour in colours)
                {
                    if (colour != 0xFFFF)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public List<byte> LoadFromGameFile(Stream file)
        {
            var coloursWithAlpha = new List<byte>();
            for (byte i = 0; i < ColourCount; i++)
            {
                colours[i] = file.ReadUShort();
                if (colours[i] != 0xFFFF && (colours[i] & AlphaBit) != 0)
                {
                    coloursWithAlpha.Add(i);
                }
            }
            return coloursWithAlpha;
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

        public void LoadFromEditableFile(Stream file)
        {
            using (var reader = new StreamReader(file))
            {
                if (reader.ReadLine() != "JASC-PAL" || reader.ReadLine() != "0100" || reader.ReadLine() != "16")
                {
                    throw new Exception("Invalid JASC palette.");
                }

                for (int i = 0; i < ColourCount; i++)
                {
                    string colourText = reader.ReadLine();
                    string[] parts = colourText.Split(' ');
                    if (parts.Length != 3)
                    {
                        throw new Exception("Invalid colour.");
                    }

                    int R = int.Parse(parts[0]) / 8;
                    int G = int.Parse(parts[1]) / 8;
                    int B = int.Parse(parts[2]) / 8;

                    int colour = (B << 10) + (G << 5) + R;
                    colours[i] = (ushort)colour;
                    string debug = $"{colour:X4}";
                }
            }
        }

        public void Empty()
        {
            for (int i = 0; i < ColourCount; i++)
            {
                colours[i] = 0xFFFF;
            }
        }

        public void WriteToGameFile(Stream file, List<byte> coloursWithAlpha)
        {
            byte i = 0;
            foreach (ushort colour in colours)
            {
                file.WriteUShort(coloursWithAlpha.Contains(i) ? (ushort)(colour | AlphaBit) : colour);
                i++;
            }
        }
    }
}