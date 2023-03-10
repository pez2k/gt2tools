using System.IO;

namespace GT1.DataSplitter
{
    public class Suspension : DataStructure
    {
        public Suspension()
        {
            Header = "SUSPENS";
            Size = 0x48;
            // 0x36: car ID
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_car{rawData[0x36]:X2}{Path.GetExtension(filename)}");
        }
    }
}