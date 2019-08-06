using System;
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
                colour.LoadFromGameFile(file, layout, i);
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

        public void WriteToEditableFiles(string directory, Stream bitmapFile)
        {
            WriteToBitmapFile(bitmapFile);
            foreach (CarColour colour in colours)
            {
                colour?.WriteToEditableFiles(directory);
            }
        }

        private void WriteToBitmapFile(Stream file)
        {
            var texture = new byte[BitmapHeight, BitmapWidth];
            GCHandle memoryHandle = GCHandle.Alloc(texture, GCHandleType.Pinned);
            using (var bitmap = new Bitmap(BitmapWidth, BitmapHeight, BitmapWidth, PixelFormat.Format4bppIndexed, memoryHandle.AddrOfPinnedObject()))
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

        public void LoadFromEditableFiles(string directory, Stream bitmapFile)
        {
            LoadFromBitmapFile(bitmapFile);
            LoadColoursFromEditableFiles(directory);
        }

        private void LoadFromBitmapFile(Stream file)
        {
            byte[] buffer;
            using (var bitmap = new Bitmap(file))
            {
                if (bitmap.Width != BitmapWidth || bitmap.Height != BitmapHeight || bitmap.PixelFormat != PixelFormat.Format4bppIndexed)
                {
                    throw new Exception("Invalid BMP.");
                }

                BitmapData rawData = bitmap.LockBits(new Rectangle(0, 0, BitmapWidth, BitmapHeight), ImageLockMode.ReadOnly, PixelFormat.Format4bppIndexed);
                buffer = new byte[rawData.Stride * BitmapHeight];
                Marshal.Copy(rawData.Scan0, buffer, 0, rawData.Stride * BitmapHeight);
                bitmap.UnlockBits(rawData);
            }

            int i = 0;
            for (int y = 0; y < BitmapHeight; y++)
            {
                for (int x = 0; x < BitmapWidth; x += 2)
                {
                    byte pixelPair = buffer[i++];
                    bitmapData[x, y] = (byte)(pixelPair >> 4);
                    bitmapData[x + 1, y] = (byte)(pixelPair & 0xF);
                }
            }
        }

        private void LoadColoursFromEditableFiles(string directory)
        {
            int i = 0;
            foreach (string colourDirectory in Directory.EnumerateDirectories(directory, "Colour??"))
            {
                var colour = new CarColour();
                colour.LoadFromEditableFiles(colourDirectory);
                colours[i++] = colour;
            }
        }

        public void WriteToGameFile(Stream file, GameFileLayout layout)
        {
            file.Position = layout.BitmapStartIndex;

            for (int y = 0; y < BitmapHeight; y++)
            {
                for (int x = 0; x < BitmapWidth; x += 2)
                {
                    file.WriteByte((byte)((bitmapData[x + 1, y] << 4) + (bitmapData[x, y] & 0xF)));
                }
            }
        }
    }
}
