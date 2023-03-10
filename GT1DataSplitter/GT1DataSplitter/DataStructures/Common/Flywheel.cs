using System.IO;

namespace GT1.DataSplitter
{
    public class Flywheel : DataStructure
    {
        public Flywheel()
        {
            Header = "FLYWHEL";
            Size = 0x14;
            // 0x4: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0x4]:X2}{Path.GetExtension(filename)}");
        }
    }
}