using System.IO;
using System.Text;

namespace GT1.DataSplitter
{
    public class TurbineKit : DataStructure
    {
        public TurbineKit()
        {
            Header = "TURBINE";
            Size = 0x38;
            // 0x1A: car ID
            // 0x1E: stage - 1
            // 0x20-2A: name, bs for stg1, bp stg2, ps stg3, pp stg4
            // 0x2C: price
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{Encoding.ASCII.GetString(rawData[0x23..0x28])}_stage{rawData[0x1E] + 1}{Path.GetExtension(filename)}");
        }
    }
}