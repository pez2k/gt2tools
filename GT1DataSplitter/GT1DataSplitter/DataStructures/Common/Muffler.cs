using System.IO;

namespace GT1.DataSplitter
{
    public class Muffler : DataStructure
    {
        public Muffler()
        {
            Header = "MUFFLER";
            Size = 0x24;
            // 0x10: car ID
            // 0x14: extra 4b value before price
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename),
                $"_car{rawData[0x10]:X2}" +
                $"_stage{rawData[0x12] + 1:X2}" +
                $"_{Parent.StringTables[0][rawData[0x1C]]}" +
                $" {Parent.StringTables[1][rawData[0x20]].Replace('/', '-')}" +
                Path.GetExtension(filename));
        }
    }
}