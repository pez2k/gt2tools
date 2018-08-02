using System.IO;

namespace TEX2CDP
{
    class Program
    {
        const uint TEX_PALETTESTART = 0x8060;
        const uint TEX_IMAGESTART = 0x60;
        const uint CDP_PALETTESTART = 0x20;
        const uint CDP_IMAGESTART = 0x43A0;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }
            
            Export(args[0]);
        }

        static void Export(string texFilename)
        {
            using (FileStream texFile = new FileStream(texFilename, FileMode.Open, FileAccess.Read))
            {
                string cdpFilename = Path.GetFileNameWithoutExtension(texFilename) + ".cdp";

                using (FileStream cdpFile = new FileStream(cdpFilename, FileMode.Open, FileAccess.ReadWrite))
                {
                    texFile.Position = 0x0E;
                    byte paletteCount = (byte)texFile.ReadByte();
                    cdpFile.Position = 0;
                    byte oldPaletteCount = (byte)cdpFile.ReadByte();
                    cdpFile.Position = 0;
                    cdpFile.WriteByte(paletteCount);

                    byte[] paletteIDs = new byte[paletteCount];
                    texFile.Position = 0x10;
                    texFile.Read(paletteIDs, 0, paletteIDs.Length);
                    cdpFile.Position = 2;
                    cdpFile.Write(paletteIDs, 0, paletteIDs.Length);
                    
                    byte[] unknownData = new byte[0x40];
                    if (paletteCount > oldPaletteCount)
                    {
                        cdpFile.Position = 0x220;
                        cdpFile.Read(unknownData, 0, unknownData.Length);
                    }

                    // Read CLUTs from TEX
                    for (byte i = 0; i < paletteCount; i++)
                    {
                        texFile.Position = TEX_PALETTESTART + (i * 0x200);
                        cdpFile.Position = CDP_PALETTESTART + (i * 0x240);
                        byte[] clutData = new byte[16 * 2 * 16]; // 16 ushorts * 16 CLUTs
                        texFile.Read(clutData, 0, clutData.Length);
                        cdpFile.Write(clutData, 0, clutData.Length);
                    }

                    int leftovers = oldPaletteCount - paletteCount;
                    if (leftovers > 0)
                    {
                        cdpFile.Position = 0x20 + (paletteCount * 0x240);
                        byte[] blankData = new byte[leftovers * 0x240];
                        cdpFile.Write(blankData, 0, blankData.Length);
                    }

                    // Read image data from TEX
                    texFile.Position = TEX_IMAGESTART + 0x1000; // Skip first 32 blank rows
                    cdpFile.Position = CDP_IMAGESTART;
                    byte[] imageData = new byte[256 * 224 / 2]; // 256 x 224 pixels at 4BPP
                    texFile.Read(imageData, 0, imageData.Length);
                    cdpFile.Write(imageData, 0, imageData.Length);
                }
            }
        }
    }
}
