using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class Palette
    {
        private ushort[] colours = new ushort[16];

        public void LoadFromGameFile(Stream file)
        {
            for (int i = 0; i < 16; i++)
            {
                colours[i] = file.ReadUShort();
            }
        }
    }
}
