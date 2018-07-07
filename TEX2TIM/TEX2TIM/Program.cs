using System.IO;

namespace GT2.TEX2TIM
{
    using StreamExtensions;

    class Program
    {
        const uint TIM_4BPP = 0;
        const uint TIM_INDEXED = 8;
        const uint TEX_PALETTESTART = 0x8060;
        const uint TEX_IMAGESTART = 0x60;

        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                return;
            }

            string filename = args[0];
            int paletteNumber = 1;

            if (args.Length > 1)
            {
                int.TryParse(args[1], out paletteNumber);
                if (paletteNumber < 1)
                {
                    paletteNumber = 1;
                }
            }

            //if (Path.GetExtension(filename) == ".cdp" || Path.GetExtension(filename) == ".cnp")
            //{
                Export(filename, paletteNumber);
            //}
            //else if (Path.GetExtension(filename) == ".tim")
            //{
                //Import(filename, paletteNumber);
            //}
        }

        static void Export(string texFilename, int paletteNumber)
        {
            using (FileStream texFile = new FileStream(texFilename, FileMode.Open, FileAccess.Read))
            {
                string timFilename = Path.GetFileNameWithoutExtension(texFilename) + "_tex" + ".tim";

                using (FileStream timFile = new FileStream(timFilename, FileMode.Create, FileAccess.Write))
                {
                    int paletteCount = texFile.ReadByte();

                    if (paletteNumber > paletteCount)
                    {
                        paletteNumber = 1;
                    }

                    // TIM header
                    timFile.Write(new byte[] { 0x10, 0x00, 0x00, 0x00 }, 0, 4);
                    timFile.WriteUInt(TIM_4BPP + TIM_INDEXED); // TIM type flags

                    // TIM CLUT header
                    timFile.WriteUInt(12 + ((16 * 2) * 16)); // Header length + CLUT size (16 ushorts) * 16 CLUTs
                    timFile.WriteUShort(0); // CLUT memory target location X
                    timFile.WriteUShort(0); // CLUT memory target location Y
                    timFile.WriteUShort(16); // Colours in CLUT
                    timFile.WriteUShort(16); // Number of CLUTs

                    // Read CLUTs from TEX
                    texFile.Position = TEX_PALETTESTART + ((paletteNumber - 1) * 0x240);
                    byte[] clutData = new byte[16 * 2 * 16]; // 16 ushorts * 16 CLUTs
                    texFile.Read(clutData, 0, clutData.Length);
                    timFile.Write(clutData, 0, clutData.Length);

                    // TIM image header
                    uint imageDataLength = 256 * 256 / 2; // 256 x 256 pixels at 4BPP
                    timFile.WriteUInt(12 + imageDataLength); // Header length + image data size
                    timFile.WriteUShort(0); // Image memory target location X
                    timFile.WriteUShort(0); // Image memory target location Y
                    timFile.WriteUShort(256 / 4); // Image width / 4
                    timFile.WriteUShort(256); // Image height - NOT divided by 4

                    // Read image data from TEX
                    texFile.Position = TEX_IMAGESTART;
                    byte[] imageRow = new byte[imageDataLength]; // 256 x 256 pixels at 4BPP
                    texFile.Read(imageRow, 0, imageRow.Length);
                    timFile.Write(imageRow, 0, imageRow.Length);
                }
            }
        }
        
        static void Import(string timFilename, int paletteNumber)
        {
            using (FileStream timFile = new FileStream(timFilename, FileMode.Open, FileAccess.Read))
            {
                string cdpFilename = Path.GetFileNameWithoutExtension(timFilename);
                if (cdpFilename.EndsWith("_cnp"))
                {
                    cdpFilename = cdpFilename.Replace("_cnp", "") + ".cnp";
                }
                else
                {
                    cdpFilename = cdpFilename.Replace("_cdp", "") + ".cdp";
                }

                using (FileStream cdpFile = new FileStream(cdpFilename, FileMode.Open, FileAccess.ReadWrite))
                {
                    int paletteCount = cdpFile.ReadByte();

                    if (paletteNumber > paletteCount)
                    {
                        paletteNumber = 1;
                    }

                    timFile.Position = 0x14;
                    byte[] clutData = new byte[16 * 2 * 16]; // 16 ushorts * 16 CLUTs
                    timFile.Read(clutData, 0, clutData.Length);
                    
                    cdpFile.Position = TEX_PALETTESTART + ((paletteNumber - 1) * 0x240);
                    cdpFile.Write(clutData, 0, clutData.Length);

                    timFile.Position = 0x220;
                    // Read image data from TIM
                    cdpFile.Position = TEX_IMAGESTART;
                    byte[] imageRow = new byte[256 * 224 / 2]; // 256 x 224 pixels at 4BPP
                    timFile.Read(imageRow, 0, imageRow.Length);
                    cdpFile.Write(imageRow, 0, imageRow.Length);
                }
            }
        }
    }
}
