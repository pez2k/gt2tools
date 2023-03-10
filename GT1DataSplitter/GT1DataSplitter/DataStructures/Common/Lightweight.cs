using System.IO;

namespace GT1.DataSplitter
{
    public class Lightweight : DataStructure
    {
        public Lightweight()
        {
            Header = "LWEIGHT";
            Size = 0x14;
            // 0x2: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0x2]:X2}{Path.GetExtension(filename)}");
        }
    }
}