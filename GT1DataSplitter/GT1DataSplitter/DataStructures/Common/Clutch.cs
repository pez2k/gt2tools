using System.IO;

namespace GT1.DataSplitter
{
    public class Clutch : DataStructure
    {
        public Clutch()
        {
            Header = "CLUTCH";
            Size = 0x18;
            // 0x6: car ID
            // 0x8: stage
            // 0x10: name part 1?
            // 0x14: name part 2?
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0x6]:X2}{Path.GetExtension(filename)}");
        }
    }
}