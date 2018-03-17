using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GT2.TextureConverter
{
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream file = new FileStream("bjjrr.cdp", FileMode.Open))
            {
                for (int clut = 0; clut < 14; clut++)
                {
                    file.Position = 0x20;

                    var texture = new byte[224, 256];
                    GCHandle memoryHandle = GCHandle.Alloc(texture, GCHandleType.Pinned);
                    var bitmap = new Bitmap(256, 224, 256, PixelFormat.Format8bppIndexed, memoryHandle.AddrOfPinnedObject());
                    ColorPalette palette = bitmap.Palette;

                    for (int i = 0; i < 224; i++)
                    {
                        ushort paletteColour = file.ReadUShort();
                        int R = paletteColour & 0x1F;
                        int G = (paletteColour >> 5) & 0x1F;
                        int B = (paletteColour >> 10) & 0x1F;

                        palette.Entries[i] = Color.FromArgb(R * 8, G * 8, B * 8);
                    }
                    bitmap.Palette = palette;

                    file.Position = 0x43A0;

                    for (int y = 0; y < 224; y++)
                    {
                        for (int x = 0; x < 256; x += 2)
                        {
                            byte pixelPair = (byte)file.ReadByte();
                            texture[y, x] = (byte)((pixelPair & 0xF) + (16 * clut));
                            texture[y, x + 1] = (byte)((pixelPair >> 4) + (16 * clut));
                        }
                    }

                    memoryHandle.Free();
                    bitmap.Save($"test{clut}.bmp");
                    bitmap.Dispose();
                }
            }
        }
    }
}
