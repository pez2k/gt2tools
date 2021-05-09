using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class CarColour
    {
        private byte colourID;
        private readonly Palette[] palettes = new Palette[16];
        private readonly IlluminationMask[] illuminationMasks = new IlluminationMask[16];
        private readonly PaintMask[] paintMasks = new PaintMask[16];
        private readonly List<(byte, byte)> coloursWithAlpha = new List<(byte, byte)>();

        public void LoadFromGameFile(Stream file, GameFileLayout layout, ushort colourNumber)
        {
            file.Position = layout.ColourCountIndex + 2 + colourNumber;
            colourID = file.ReadSingleByte();
            file.Position = layout.PaletteStartIndex + (layout.PaletteSize * colourNumber);
            for (byte i = 0; i < 16; i++)
            {
                var palette = new Palette();
                List<byte> paletteColoursWithAlpha = palette.LoadFromGameFile(file);
                foreach (byte colourID in paletteColoursWithAlpha)
                {
                    coloursWithAlpha.Add((i, colourID));
                }
                palettes[i] = palette;
            }

            if (layout.SingleInstanceOfFlagsStartIndex != 0)
            {
                if (colourNumber > 0)
                {
                    return;
                }
                file.Position = layout.SingleInstanceOfFlagsStartIndex;
            }

            for (int i = 0; i < 16; i++)
            {
                var illuminationMask = new IlluminationMask();
                illuminationMask.LoadFromGameFile(file);
                illuminationMasks[i] = illuminationMask;
            }
            for (int i = 0; i < 16; i++)
            {
                var paintMask = new PaintMask();
                paintMask.LoadFromGameFile(file);
                paintMasks[i] = paintMask;
            }
        }

        public Palette GetPalette(int number) => palettes[number];

        public void WriteToEditableFiles(string directory)
        {
            directory = Path.Combine(directory, $"Colour{colourID:X2}");
            Directory.CreateDirectory(directory);
            WritePalettesToEditableFiles(directory);
            WriteIlluminationMasksToEditableFiles(directory);
            WritePaintMasksToEditableFiles(directory);
            WriteAlphaBitsToEditableFile(directory);
        }

        private void WritePalettesToEditableFiles(string directory)
        {
            for (int i = 0; i < palettes.Length; i++)
            {
                if (!palettes[i].IsEmpty)
                {
                    using (var file = new FileStream(Path.Combine(directory, $"ColourPalette{i:D2}.pal"), FileMode.Create, FileAccess.Write))
                    {
                        palettes[i].WriteToJASCPalette(file);
                    }
                }
            }
        }

        private void WriteIlluminationMasksToEditableFiles(string directory)
        {
            for (int i = 0; i < illuminationMasks.Length; i++)
            {
                if (illuminationMasks[i]?.IsEmpty == false)
                {
                    using (var file = new FileStream(Path.Combine(directory, $"IlluminationMask{i:D2}.pal"), FileMode.Create, FileAccess.Write))
                    {
                        illuminationMasks[i].WriteToJASCPalette(file);
                    }
                }
            }
        }

        private void WritePaintMasksToEditableFiles(string directory)
        {
            for (int i = 0; i < paintMasks.Length; i++)
            {
                if (paintMasks[i]?.IsEmpty == false)
                {
                    using (var file = new FileStream(Path.Combine(directory, $"PaintMask{i:D2}.pal"), FileMode.Create, FileAccess.Write))
                    {
                        paintMasks[i].WriteToJASCPalette(file);
                    }
                }
            }
        }

        private void WriteAlphaBitsToEditableFile(string directory)
        {
            if (coloursWithAlpha.Count > 0)
            {
                using (var file = new StreamWriter(Path.Combine(directory, $"SetAlphaBits.txt")))
                {
                    foreach ((byte, byte) colour in coloursWithAlpha)
                    {
                        file.WriteLine($"{colour.Item1:D2},{colour.Item2:D2}");
                    }
                }
            }
        }

        public void LoadFromEditableFiles(string directory)
        {
            colourID = byte.Parse(Path.GetFileName(directory).Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            string alphaBitsPath = Path.Combine(directory, "SetAlphaBits.txt");
            if (File.Exists(alphaBitsPath))
            {
                using (var file = new StreamReader(alphaBitsPath))
                {
                    for (byte i = 0; i <= 255; i++)
                    {
                        string alphaBit = file.ReadLine();
                        if (alphaBit == null)
                        {
                            break;
                        }
                        string[] bits = alphaBit.Split(',');
                        coloursWithAlpha.Add((byte.Parse(bits[0]), byte.Parse(bits[1])));
                    }
                }
            }

            foreach (string palettePath in Directory.EnumerateFiles(directory, "ColourPalette??.pal"))
            {
                byte number = byte.Parse(Path.GetFileName(palettePath).Substring(13, 2));
                var palette = new Palette();
                using (var file = new FileStream(palettePath, FileMode.Open, FileAccess.Read))
                {
                    palette.LoadFromEditableFile(file);
                }
                palettes[number] = palette;
            }

            for (int i = 0; i < palettes.Length; i++)
            {
                if (palettes[i] == null)
                {
                    palettes[i] = new Palette();
                    palettes[i].Empty();
                }
            }

            foreach (string illuminationMaskPath in Directory.EnumerateFiles(directory, "IlluminationMask??.pal"))
            {
                byte number = byte.Parse(Path.GetFileName(illuminationMaskPath).Substring(16, 2));
                var illuminationMask = new IlluminationMask();
                using (var file = new FileStream(illuminationMaskPath, FileMode.Open, FileAccess.Read))
                {
                    illuminationMask.LoadFromEditableFile(file);
                }
                illuminationMasks[number] = illuminationMask;
            }

            for (int i = 0; i < illuminationMasks.Length; i++)
            {
                if (illuminationMasks[i] == null)
                {
                    illuminationMasks[i] = new IlluminationMask();
                }
            }

            foreach (string paintMaskPath in Directory.EnumerateFiles(directory, "PaintMask??.pal"))
            {
                byte number = byte.Parse(Path.GetFileName(paintMaskPath).Substring(9, 2));
                var paintMask = new PaintMask();
                using (var file = new FileStream(paintMaskPath, FileMode.Open, FileAccess.Read))
                {
                    paintMask.LoadFromEditableFile(file);
                }
                paintMasks[number] = paintMask;
            }

            for (int i = 0; i < paintMasks.Length; i++)
            {
                if (paintMasks[i] == null)
                {
                    paintMasks[i] = new PaintMask();
                }
            }
        }

        public void WriteToGameFile(Stream file, GameFileLayout layout, ushort colourNumber)
        {
            file.Position = layout.ColourCountIndex + 2 + colourNumber;
            file.WriteByte(colourID);
            file.Position = layout.PaletteStartIndex + (layout.PaletteSize * colourNumber);
            int i = 0;
            foreach (Palette palette in palettes)
            {
                List<byte> paletteColoursWithAlpha = coloursWithAlpha.Where(colour => colour.Item1 == i).Select(colour => colour.Item2).ToList();
                palette.WriteToGameFile(file, paletteColoursWithAlpha);
                i++;
            }

            if (layout.SingleInstanceOfFlagsStartIndex != 0)
            {
                if (colourNumber > 0)
                {
                    return;
                }
                file.Position = layout.SingleInstanceOfFlagsStartIndex;
            }

            foreach (IlluminationMask illuminationMask in illuminationMasks)
            {
                illuminationMask?.WriteToGameFile(file);
            }
            foreach (PaintMask paintMask in paintMasks)
            {
                paintMask?.WriteToGameFile(file);
            }
        }
    }
}
