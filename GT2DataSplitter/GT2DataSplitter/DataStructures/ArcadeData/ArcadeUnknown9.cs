using GT2.CarNameConversion;
using GT2.StreamExtensions;

namespace GT2.DataSplitter
{
    public class ArcadeUnknown9 : DataStructure
    {
        public ArcadeUnknown9()
        {
            Size = 0x10;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + data.ReadUInt().ToCarName() + ".dat";
        }
    }
}
