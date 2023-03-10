using System.IO;

namespace GT1.DataSplitter
{
    public class Lightweight : DataStructure
    {
        public Lightweight()
        {
            Header = "LWEIGHT";
            Size = 0x14;
            // 0x1: 1b stage instead of normal 2b after car ID
            // 0x2: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename),
                $"_car{rawData[0x2]:X2}" +
                $"_stage{rawData[0x1] + 1:X2}" +
                $"_{Parent.StringTables[0][rawData[0x8]]}" +
                $" {Parent.StringTables[1][rawData[0xC]].Replace('/', '-')}" +
                Path.GetExtension(filename));
        }
    }
}