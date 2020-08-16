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
        private const int PaletteSize = 16;
        private const int ColumnWidth = 256 / 2;
        private const int RightColumnWidth = 128 / 2;
        private const int TopRowHeight = 256;
        private const int BottomRowHeight = 224;
        private const int BitmapWidth = (ColumnWidth + ColumnWidth + RightColumnWidth) * 2;
        private const int BitmapHeight = TopRowHeight + BottomRowHeight;
        private const ushort AlphaFlag = 0x8000;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            string path = args.Last();
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                Extract(path);
            }
            else if (Path.GetExtension(path) == ".bmp")
            {
                Rebuild(path);
            }
        }

        private static void Extract(string directory)
        {
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
                                int R = (paletteColour & 0x1F) * 8;
                                int G = ((paletteColour >> 5) & 0x1F) * 8;
                                int B = ((paletteColour >> 10) & 0x1F) * 8;

                                if ((paletteColour & AlphaFlag) > 0 && B < 255)
                                {
                                    B++;
                                }

                                palette.Entries[i] = Color.FromArgb(R, G, B);
                            }
                            bitmap.Palette = palette;
                        }
                        else
                        {
                            (int tileWidth, int tileHeight, int startX, int startY) = GetTileInfo(fileNumber);
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

        private static void Rebuild(string bitmapPath)
        {
            string outputDirectory = Path.GetFileNameWithoutExtension(bitmapPath) + ".slt";
            Directory.CreateDirectory(outputDirectory);
            byte[] bitmapData;
            using (var bitmapFile = new FileStream(bitmapPath, FileMode.Open, FileAccess.Read))
            {
                using (var bitmap = new Bitmap(bitmapFile))
                {
                    Assert(bitmap.Width == BitmapWidth && bitmap.Height == BitmapHeight && bitmap.PixelFormat == PixelFormat.Format4bppIndexed,
                           "Invalid BMP - must be 640x480 16-colour.");

                    BitmapData rawData = bitmap.LockBits(new Rectangle(0, 0, BitmapWidth, BitmapHeight), ImageLockMode.ReadOnly, PixelFormat.Format4bppIndexed);
                    bitmapData = new byte[rawData.Stride * BitmapHeight];
                    Marshal.Copy(rawData.Scan0, bitmapData, 0, rawData.Stride * BitmapHeight);
                    bitmap.UnlockBits(rawData);

                    using (var paletteFile = new FileStream(Path.Combine(outputDirectory, "000.dat"), FileMode.Create, FileAccess.Write))
                    {
                        ColorPalette palette = bitmap.Palette;
                        foreach (Color colour in palette.Entries)
                        {
                            int R = colour.R / 8;
                            int G = colour.G / 8;
                            int B = colour.B / 8;
                            ushort colourValue = (ushort)((B << 10) + (G << 5) + R);

                            if (colour.B % 8 == 1)
                            {
                                colourValue += AlphaFlag;
                            }

                            paletteFile.WriteUShort(colourValue);
                        }
                    }
                }
            }

            for (int fileNumber = 1; fileNumber < 7; fileNumber++)
            {
                using (var file = new FileStream(Path.Combine(outputDirectory, $"{fileNumber:D3}.dat"), FileMode.Create, FileAccess.Write))
                {
                    (int tileWidth, int tileHeight, int startX, int startY) = GetTileInfo(fileNumber);
                    for (int y = startY; y < startY + tileHeight; y++)
                    {
                        for (int x = startX; x < startX + tileWidth; x++)
                        {
                            byte pixelPair = bitmapData[x + (y * BitmapWidth / 2)];
                            file.WriteByte(SwapByteHalves(pixelPair));
                        }
                    }
                }
            }
        }

        private static (int tileWidth, int tileHeight, int startX, int startY) GetTileInfo(int fileNumber)
        {
            int tileWidth = fileNumber % 3 == 0 ? RightColumnWidth : ColumnWidth;
            int tileHeight = fileNumber > 3 ? BottomRowHeight : TopRowHeight;
            int startX = (fileNumber - 1) % 3 * ColumnWidth;
            int startY = fileNumber > 3 ? TopRowHeight : 0;
            return (tileWidth, tileHeight, startX, startY);
        }

        private static byte SwapByteHalves(byte input) => (byte)(((input & 0xF) << 4) + ((input >> 4) & 0xF));

        private static void Assert(bool condition, string message = null)
        {
            if (!condition)
            {
                throw new Exception(message ?? "Assertion failed.");
            }
        }
    }
}
