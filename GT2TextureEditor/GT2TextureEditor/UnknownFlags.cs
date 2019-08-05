using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class UnknownFlags
    {
        private readonly bool[] colours = new bool[16];

        public void LoadFromGameFile(Stream file)
        {
            ushort flags = file.ReadUShort();
            for (int i = 0; i < 16; i++)
            {
                colours[i] = (flags & 1) == 1;
                flags = (ushort)(flags >> 1);
            }
        }
    }
}
