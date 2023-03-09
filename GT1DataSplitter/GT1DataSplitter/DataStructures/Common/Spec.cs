using System.IO;
using System.Text;

namespace GT1.DataSplitter
{
    public class Spec : DataStructure
    {
        public Spec()
        {
            Header = "SPEC";
            Size = 0x1A8;
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{Encoding.ASCII.GetString(rawData[..5])}{Path.GetExtension(filename)}");
        }
    }
}