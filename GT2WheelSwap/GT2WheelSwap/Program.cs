using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2WheelSwap
{
    class Program
    {
        static void Main(string[] args)
        {
            Export();
        }

        static void Export()
        {
            const uint TIM_4BPP = 0;
            const uint TIM_INDEXED = 8;
            const uint CDP_PALETTESTART = 0x20;
            const uint CDP_IMAGESTART = 0x43A0;

            using (FileStream cdpFile = new FileStream("bjjrr.cdp", FileMode.Open, FileAccess.Read))
            {
                using (FileStream timFile = new FileStream("output.tim", FileMode.Create, FileAccess.Write))
                {
                    // TIM header
                    timFile.Write(new byte[] { 0x10, 0x00, 0x00, 0x00 }, 0, 4);
                    timFile.WriteUInt(TIM_4BPP + TIM_INDEXED); // TIM type flags

                    // TIM CLUT header
                    timFile.WriteUInt(12 + (16 * 2)); // Header length + CLUT size (16 ushorts)
                    timFile.WriteUShort(0); // CLUT memory target location X
                    timFile.WriteUShort(0); // CLUT memory target location Y
                    timFile.WriteUShort(16); // Colours in CLUT
                    timFile.WriteUShort(1); // Number of CLUTs

                    // Read CLUT from CDP
                    cdpFile.Position = CDP_PALETTESTART;
                    byte[] clutData = new byte[16 * 2]; // 16 ushorts
                    cdpFile.Read(clutData, 0, clutData.Length);
                    timFile.Write(clutData, 0, clutData.Length);

                    // TIM image header
                    timFile.WriteUInt(12 + (48 * 48 / 2)); // Header length + image data size (48 x 48 pixels at 4BPP)
                    timFile.WriteUShort(0); // Image memory target location X
                    timFile.WriteUShort(0); // Image memory target location Y
                    timFile.WriteUShort(48 / 4); // Image width / 4
                    timFile.WriteUShort(48); // Image height - NOT divided by 4

                    // Read image data from CDP
                    for (int y = 0; y < 48; y++)
                    {
                        cdpFile.Position = CDP_IMAGESTART + (y * 256 / 2); // 256 pixel width at 4BPP
                        byte[] imageRow = new byte[48 / 2]; // 48 pixels at 4BPP
                        cdpFile.Read(imageRow, 0, imageRow.Length);
                        timFile.Write(imageRow, 0, imageRow.Length);
                    }
                }
            }
        }
    }
}
