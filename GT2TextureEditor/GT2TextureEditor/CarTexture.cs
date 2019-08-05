using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class CarTexture
    {
        private CarColour[] colours = new CarColour[16];
        private byte[] bitmapData = new byte[256 * 224 * 16];

        public void LoadFromGameFile(Stream file, GameFileLayout layout)
        {
            file.Position = layout.ColourCountIndex;
            ushort colourCount = file.ReadUShort();
            for (ushort i = 0; i < colourCount; i++)
            {
                var colour = new CarColour();
                colour.LoadFromGameFile(file, layout);
                colours[i] = colour;
            }

            file.Position = layout.BitmapStartIndex;
            file.Read(bitmapData);
        }
    }
}
