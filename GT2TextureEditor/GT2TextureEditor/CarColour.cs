using System.Drawing.Imaging;
using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class CarColour
    {
        private byte colourID;
        private readonly Palette[] palettes = new Palette[16];
        private readonly Illumination[] illuminations = new Illumination[16];
        private readonly UnknownFlags[] unknowns = new UnknownFlags[16];

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
                var illumination = new Illumination();
                illumination.LoadFromGameFile(file);
                illuminations[i] = illumination;
            }
            for (int i = 0; i < 16; i++)
            {
                var unknown = new UnknownFlags();
                unknown.LoadFromGameFile(file);
                unknowns[i] = unknown;
            }
            file.Position = idPosition + 1;
        }

        public void WriteFirstPaletteToBitmapPalette(ColorPalette palette) => palettes[0].WriteToBitmapPalette(palette);

        public void WritePalettesToEditableFiles()
        {
            for (int i = 0; i < palettes.Length; i++)
            {
                using (var file = new FileStream($"palette{i:D2}.pal", FileMode.Create, FileAccess.Write))
                {
                    palettes[i].WriteToJASCPalette(file);
                }
            }
        }

        public void WriteIlluminationToEditableFiles()
        {
            for (int i = 0; i < illuminations.Length; i++)
            {
                using (var file = new FileStream($"illumination{i:D2}.pal", FileMode.Create, FileAccess.Write))
                {
                    illuminations[i].WriteToJASCPalette(file);
                }
            }
        }

        public void WriteUnknownsToEditableFiles()
        {
            for (int i = 0; i < unknowns.Length; i++)
            {
                using (var file = new FileStream($"unknown{i:D2}.pal", FileMode.Create, FileAccess.Write))
                {
                    unknowns[i].WriteToJASCPalette(file);
                }
            }
        }
    }
}
