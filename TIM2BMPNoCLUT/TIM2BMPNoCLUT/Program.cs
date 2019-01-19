using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GT2.TIM2BMPNoCLUT
{
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream file = new FileStream("arc_goodies_uk.tim", FileMode.Open))
            {
                file.ReadUInt(); // 0x10
                file.ReadUInt(); // 0x00
                file.ReadUInt(); // image data size
                file.ReadUShort(); // memory pos
                file.ReadUShort(); // memory pos
                ushort x = (ushort)(file.ReadUShort() * 4); // X size / 4
                ushort y = file.ReadUShort(); // Y size

                //var texture = new byte[y, x];
                //GCHandle memoryHandle = GCHandle.Alloc(texture, GCHandleType.Pinned);
                //var bitmap = new Bitmap(x, y, x, PixelFormat.DontCare, memoryHandle.AddrOfPinnedObject());
                var bitmap = new Bitmap(x, y);

                for (int yi = 0; yi < y; yi++)
                {
                    for (int xi = 0; xi < x; xi++)
                    {
                        ushort paletteColour = file.ReadUShort();
                        int R = paletteColour & 0x1F;
                        int G = (paletteColour >> 5) & 0x1F;
                        int B = (paletteColour >> 10) & 0x1F;
                        
                        var colour = Color.FromArgb(R * 8, G * 8, B * 8);
                        bitmap.SetPixel(xi, yi, colour);
                    }
                }

                //memoryHandle.Free();
                bitmap.Save($"test.bmp");
                bitmap.Dispose();
            }
        }
    }
}