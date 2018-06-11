using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2.BGRTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            bool toBGR = false;
            string input = args[0];

            if (args.Length == 2)
            {
                toBGR = (args[0].ToUpper() == "BGR");
                input = args[1];
            }

            int value;
            if (!int.TryParse(input, NumberStyles.HexNumber, null, out value))
            {
                return;
            }

            if (toBGR)
            {
                int R = (((value & 0xFF0000) >> 16) / 8) & 0x1F;
                int G = ((((value & 0x00FF00) >> 8) / 8) & 0x1F) << 5;
                int B = ((((value & 0x0000FF)) / 8) & 0x1F) << 10;
                Console.WriteLine($"{string.Format("{0:X4}", (ushort)(R + G + B))} / {R}, {G}, {B}");
            }
            else
            {
                int R = value & 0x1F;
                int G = (value >> 5) & 0x1F;
                int B = (value >> 10) & 0x1F;
                Console.WriteLine($"#{R * 8:X2}{G * 8:X2}{B * 8:X2} / {R * 8}, {G * 8}, {B * 8}");
            }
        }
    }
}
