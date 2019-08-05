using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class CarTexture
    {
        private const byte ColourCount = 16;
        private const ushort BitmapWidth = 256;
        private const byte BitmapHeight = 224;

        private readonly CarColour[] colours = new CarColour[ColourCount];
        private readonly byte[,] bitmapData = new byte[BitmapWidth, BitmapHeight];

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

            for (int y = 0; y < BitmapHeight; y++)
            {
                for (int x = 0; x < BitmapWidth; x += 2)
                {
                    byte pixelPair = (byte)file.ReadByte();
                    bitmapData[x, y] = (byte)(pixelPair & 0xF);
                    bitmapData[x + 1, y] = (byte)(pixelPair >> 4);
                }
            }
        }

        public void WriteToEditableFiles(Stream bitmapFile)
        {
            WriteToBitmapFile(bitmapFile);
            colours[0].WritePalettesToEditableFiles();
            colours[0].WriteIlluminationToEditableFiles();
            colours[0].WriteUnknownsToEditableFiles();
        }

        private void WriteToBitmapFile(Stream file)
        {
            var texture = new byte[BitmapHeight, BitmapWidth];
            GCHandle memoryHandle = GCHandle.Alloc(texture, GCHandleType.Pinned);
            using (var bitmap = new Bitmap(BitmapWidth, BitmapHeight, 256, PixelFormat.Format4bppIndexed, memoryHandle.AddrOfPinnedObject()))
            {
                ColorPalette palette = bitmap.Palette;
                colours[0].WriteFirstPaletteToBitmapPalette(palette);
                bitmap.Palette = palette;

                for (int x = 0; x < BitmapWidth; x += 2)
                {
                    for (int y = 0; y < BitmapHeight; y++)
                    {
                        texture[y, x / 2] = (byte)((bitmapData[x, y] << 4) + (bitmapData[x + 1, y] & 0xF));
                    }
                }

                memoryHandle.Free();
                bitmap.Save(file, ImageFormat.Bmp);
            }
        }
    }
}
