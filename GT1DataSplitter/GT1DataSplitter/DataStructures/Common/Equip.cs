using System.IO;
using System.Text;

namespace GT1.DataSplitter
{
    public abstract class Equip : DataStructure
    {
        public Equip()
        {
            Header = "EQUIP";
            Size = 0x6C;
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{Encoding.ASCII.GetString(rawData[0x60..0x67])}{Path.GetExtension(filename)}");
        }
    }
}