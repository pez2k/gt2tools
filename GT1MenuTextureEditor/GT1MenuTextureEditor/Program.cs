using System;
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
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory)) {
                Extract(path);
            }
        }

        private static void Extract(string directory) {
            const int PaletteSize = 16;
            const int ColumnWidth = 256 / 2;
            const int RightColumnWidth = 128 / 2;
            const int TopRowHeight = 256;
            const int BottomRowHeight = 224;
            const int BitmapWidth = (ColumnWidth + ColumnWidth + RightColumnWidth) * 2;
            const int BitmapHeight = TopRowHeight + BottomRowHeight;

            string[] filenames = Directory.EnumerateFiles(directory).ToArray();
            Assert(filenames.Length == 7, $"Expected 7 files in folder, found {filenames.Length}.");
            Assert(new FileInfo(filenames[0]).Length == PaletteSize * 2);
            Assert(new FileInfo(filenames[1]).Length == ColumnWidth * TopRowHeight);
            Assert(new FileInfo(filenames[2]).Length == ColumnWidth * TopRowHeight);
            Assert(new FileInfo(filenames[3]).Length == RightColumnWidth * TopRowHeight);
            Assert(new FileInfo(filenames[4]).Length == ColumnWidth * BottomRowHeight);
            Assert(new FileInfo(filenames[5]).Length == ColumnWidth * BottomRowHeight);
            Assert(new FileInfo(filenames[6]).Length == RightColumnWidth * BottomRowHeight);
            
            var texture = new byte[BitmapHeight, BitmapWidth];
            GCHandle memoryHandle = GCHandle.Alloc(texture, GCHandleType.Pinned);
            using (var bitmap = new Bitmap(BitmapWidth, BitmapHeight, BitmapWidth, PixelFormat.Format4bppIndexed, memoryHandle.AddrOfPinnedObject()))
            {
                for (int fileNumber = 0; fileNumber < filenames.Length; fileNumber++)
                {
                    using (var stream = new FileStream(filenames[fileNumber], FileMode.Open, FileAccess.Read))
                    {
                        if (fileNumber == 0)
                        {
                            ColorPalette palette = bitmap.Palette;
                            for (int i = 0; i < PaletteSize; i++)
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
                            int tileWidth = fileNumber % 3 == 0 ? RightColumnWidth : ColumnWidth;
                            int tileHeight = fileNumber > 3 ? BottomRowHeight : TopRowHeight;
                            int startX = (fileNumber - 1) % 3 * ColumnWidth;
                            int startY = fileNumber > 3 ? TopRowHeight : 0;
                            for (int y = startY; y < startY + tileHeight; y++)
                            {
                                var row = new byte[tileWidth];
                                stream.Read(row);
                                for (int x = 0; x < tileWidth; x++)
                                {
                                    texture[y, startX + x] = SwapByteHalves(row[x]);
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

        private static byte SwapByteHalves(byte input) => (byte)(((input & 0xF) << 4) + ((input >> 4) & 0xF));

        private static void Assert(bool condition, string message = null) {
            if (!condition)
            {
                throw new Exception(message ?? "Assertion failed.");
            }
        }
    }
}
