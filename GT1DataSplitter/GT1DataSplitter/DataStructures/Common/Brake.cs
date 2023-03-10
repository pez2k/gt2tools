using System.IO;

namespace GT1.DataSplitter
{
    public class Brake : DataStructure
    {
        public Brake()
        {
            Header = "BRAKE";
            Size = 0x18;
            // 0x4: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0x4]:X2}{Path.GetExtension(filename)}");
        }
    }
}