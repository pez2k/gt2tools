using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class CarColour
    {
        private byte colourID;
        private Palette[] palettes = new Palette[16];
        private Illumination[] illuminations = new Illumination[16];
        private UnknownFlags[] unknowns = new UnknownFlags[16];

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
    }
}
