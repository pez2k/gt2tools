using System.Drawing.Imaging;
using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class CarColour
    {
        private const ushort PaletteSize = 0x240;

        private byte colourID;
        private readonly Palette[] palettes = new Palette[16];
        private readonly IlluminationMask[] illuminationMasks = new IlluminationMask[16];
        private readonly PaintMask[] paintMasks = new PaintMask[16];

        public void LoadFromGameFile(Stream file, GameFileLayout layout, ushort colourNumber)
        {
            file.Position = layout.ColourCountIndex + 2 + colourNumber;
            colourID = file.ReadSingleByte();
            file.Position = layout.PaletteStartIndex + (PaletteSize * colourNumber);
            for (int i = 0; i < 16; i++)
            {
                var palette = new Palette();
                palette.LoadFromGameFile(file);
                palettes[i] = palette;
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

        public void WriteFirstPaletteToBitmapPalette(ColorPalette palette) => palettes[0].WriteToBitmapPalette(palette);

        public void WriteToEditableFiles(string directory)
        {
            directory = Path.Combine(directory, $"Colour{colourID:X2}");
            Directory.CreateDirectory(directory);
            WritePalettesToEditableFiles(directory);
            WriteIlluminationMasksToEditableFiles(directory);
            WritePaintMasksToEditableFiles(directory);
        }

        public void WritePalettesToEditableFiles(string directory)
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

        public void WriteIlluminationMasksToEditableFiles(string directory)
        {
            for (int i = 0; i < illuminationMasks.Length; i++)
            {
                if (!illuminationMasks[i].IsEmpty)
                {
                    using (var file = new FileStream(Path.Combine(directory, $"IlluminationMask{i:D2}.pal"), FileMode.Create, FileAccess.Write))
                    {
                        illuminationMasks[i].WriteToJASCPalette(file);
                    }
                }
            }
        }

        public void WritePaintMasksToEditableFiles(string directory)
        {
            for (int i = 0; i < paintMasks.Length; i++)
            {
                if (!paintMasks[i].IsEmpty)
                {
                    using (var file = new FileStream(Path.Combine(directory, $"PaintMask{i:D2}.pal"), FileMode.Create, FileAccess.Write))
                    {
                        paintMasks[i].WriteToJASCPalette(file);
                    }
                }
            }
        }

        public void LoadFromEditableFiles(string directory)
        {
            colourID = byte.Parse(Path.GetFileName(directory).Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

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
            file.Position = layout.PaletteStartIndex + (PaletteSize * colourNumber);
            foreach (Palette palette in palettes)
            {
                palette.WriteToGameFile(file);
            }
            foreach (IlluminationMask illuminationMask in illuminationMasks)
            {
                illuminationMask.WriteToGameFile(file);
            }
            foreach (PaintMask paintMask in paintMasks)
            {
                paintMask.WriteToGameFile(file);
            }
        }
    }
}
