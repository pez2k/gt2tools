using System.IO;
using System.Text;

namespace GT1.DataSplitter
{
    public abstract class Equip : DataStructure
    {
        public Equip()
        {
            Header = "EQUIP";
            Size = 0x6C;
            // 0x02: 00 for GT mode, 01 for Arcade?
            // 0x2A: tires
            // 0x2E: gear ratios, 1-7
            // 0x3C: final drive ratio
            // 0x43: camber F
            // 0x44: ride height F
            // 0x45: spring F
            // 0x50: camber R
            // 0x51: ride height R
            // 0x52: spring R
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{Encoding.ASCII.GetString(rawData[0x60..0x67])}{Path.GetExtension(filename)}");
        }
    }
}