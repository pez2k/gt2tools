using System.IO;

namespace GT1.DataSplitter
{
    public class Gear : DataStructure
    {
        public Gear()
        {
            Header = "GEAR";
            Size = 0x44;
            // 0x32: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0x32]:X2}{Path.GetExtension(filename)}");
        }
    }
}