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
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0x10]:X2}{Path.GetExtension(filename)}");
        }
    }
}