using System.IO;

namespace GT1.DataSplitter
{
    public class BrakeController : DataStructure
    {
        public BrakeController()
        {
            Header = "BRKCTRL";
            Size = 0x1C;
            // 0xA: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0xA]:X2}{Path.GetExtension(filename)}");
        }
    }
}