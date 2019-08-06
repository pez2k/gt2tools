using System.Drawing.Imaging;
using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class CarColour
    {
        private byte colourID;
        private readonly Palette[] palettes = new Palette[16];
        private readonly IlluminationMask[] illuminationMasks = new IlluminationMask[16];
        private readonly PaintMask[] paintMasks = new PaintMask[16];

        public void LoadFromGameFile(Stream file, GameFileLayout layout)
        {
            long idPosition = file.Position;
            colourID = file.ReadSingleByte();
            file.Position = layout.PaletteStartIndex;
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
            file.Position = idPosition + 1;
        }

        public void WriteFirstPaletteToBitmapPalette(ColorPalette palette) => palettes[0].WriteToBitmapPalette(palette);

        public void WritePalettesToEditableFiles()
        {
            for (int i = 0; i < palettes.Length; i++)
            {
                if (!palettes[i].IsEmpty)
                {
                    using (var file = new FileStream($"ColourPalette{i:D2}.pal", FileMode.Create, FileAccess.Write))
                    {
                        palettes[i].WriteToJASCPalette(file);
                    }
                }
            }
        }

        public void WriteIlluminationMasksToEditableFiles()
        {
            for (int i = 0; i < illuminationMasks.Length; i++)
            {
                if (!illuminationMasks[i].IsEmpty)
                {
                    using (var file = new FileStream($"IlluminationMask{i:D2}.pal", FileMode.Create, FileAccess.Write))
                    {
                        illuminationMasks[i].WriteToJASCPalette(file);
                    }
                }
            }
        }

        public void WritePaintMasksToEditableFiles()
        {
            for (int i = 0; i < paintMasks.Length; i++)
            {
                if (!paintMasks[i].IsEmpty)
                {
                    using (var file = new FileStream($"PaintMask{i:D2}.pal", FileMode.Create, FileAccess.Write))
                    {
                        paintMasks[i].WriteToJASCPalette(file);
                    }
                }
            }
        }
    }
}
