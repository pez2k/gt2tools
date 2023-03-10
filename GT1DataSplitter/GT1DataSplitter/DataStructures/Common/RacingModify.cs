using System.IO;

namespace GT1.DataSplitter
{
    public class RacingModify : DataStructure
    {
        public RacingModify()
        {
            Header = "RACING";
            Size = 0x20;
            // 0xE: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0xE]:X2}{Path.GetExtension(filename)}");
        }
    }
}