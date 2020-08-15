using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using StreamExtensions;

namespace GT1.MenuTextureEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            string path = args.Last();
            bool isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            if (isDirectory) {
                Extract(path);
            }
        }

        private static void Extract(string directory) {
            const int ColumnWidth = 256;
            const int RightColumnWidth = 128;
            const int TopRowHeight = 256;
            const int BottomRowHeight = 224;
            const int BitmapWidth = ColumnWidth + ColumnWidth + RightColumnWidth;
            const int BitmapHeight = TopRowHeight + BottomRowHeight;
            string[] filenames = Directory.EnumerateFiles(directory).ToArray();
            Assert(filenames.Length == 7, $"Expected 7 files in folder, found {filenames.Length}.");
            Assert(new FileInfo(filenames[0]).Length == 32);
            Assert(new FileInfo(filenames[1]).Length == ColumnWidth * TopRowHeight / 2);
            Assert(new FileInfo(filenames[2]).Length == ColumnWidth * TopRowHeight / 2);
            Assert(new FileInfo(filenames[3]).Length == RightColumnWidth * TopRowHeight / 2);
            Assert(new FileInfo(filenames[4]).Length == ColumnWidth * BottomRowHeight / 2);
            Assert(new FileInfo(filenames[5]).Length == ColumnWidth * BottomRowHeight / 2);
            Assert(new FileInfo(filenames[6]).Length == RightColumnWidth * BottomRowHeight / 2);
            
            var texture = new byte[BitmapHeight, BitmapWidth];
            GCHandle memoryHandle = GCHandle.Alloc(texture, GCHandleType.Pinned);
            using (var bitmap = new Bitmap(BitmapWidth, BitmapHeight, BitmapWidth, PixelFormat.Format4bppIndexed, memoryHandle.AddrOfPinnedObject()))
            {
                /*
                for (ushort x = 0; x < BitmapWidth; x += 2)
                {
                    for (ushort y = 0; y < BitmapHeight; y++)
                    {
                        texture[y, x / 2] = (byte)((bitmapData[x, y] << 4) + (bitmapData[x + 1, y] & 0xF));
                    }
                }*/

                for (int fileNumber = 0; fileNumber < filenames.Length; fileNumber++)
                {
                    using (var stream = new FileStream(filenames[fileNumber], FileMode.Open, FileAccess.Read))
                    {
                        if (fileNumber == 0)
                        {
                            ColorPalette palette = bitmap.Palette;
                            for (int i = 0; i < 16; i++)
                            {
                                ushort paletteColour = stream.ReadUShort();
                                int R = paletteColour & 0x1F;
                                int G = (paletteColour >> 5) & 0x1F;
                                int B = (paletteColour >> 10) & 0x1F;

                                palette.Entries[i] = Color.FromArgb(R * 8, G * 8, B * 8);
                            }
                            bitmap.Palette = palette;
                        }
                        else
                        {
                            int tileWidth = (fileNumber % 3 == 0 ? RightColumnWidth : ColumnWidth) / 2;
                            int tileHeight = fileNumber > 3 ? BottomRowHeight : TopRowHeight;
                            int startX = (fileNumber - 1) % 3 * 256 / 2;
                            int startY = fileNumber > 3 ? 256 : 0;
                            for (int y = startY; y < startY + tileHeight; y++)
                            {
                                var row = new byte[tileWidth];
                                stream.Read(row);
                                for (int x = 0; x < tileWidth; x++)
                                {
                                    byte pixelPair = row[x];
                                    byte b1 = (byte)(pixelPair & 0xF);
                                    byte b2 = (byte)(pixelPair >> 4);
                                    texture[y, startX + x] = (byte)((b1 << 4) + (b2 & 0xF));
                                }
                            }
                        }
                    }
                }

                memoryHandle.Free();

                using (var file = new FileStream($"{Path.GetFileNameWithoutExtension(directory)}.bmp", FileMode.Create, FileAccess.Write))
                {
                    bitmap.Save(file, ImageFormat.Bmp);
                }
            }
        }

        private static void Assert(bool condition, string message = null) {
            if (!condition)
            {
                throw new Exception(message ?? "Assertion failed.");
            }
        }
    }
}
