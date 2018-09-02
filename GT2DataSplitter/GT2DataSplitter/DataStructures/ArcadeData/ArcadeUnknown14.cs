using GT2.CarNameConversion;
using GT2.StreamExtensions;

namespace GT2.DataSplitter
{
    public class ArcadeUnknown14 : DataStructure
    {
        public ArcadeUnknown14()
        {
            Size = 0x3C;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + data.ReadUInt().ToCarName() + ".dat";
        }
    }
}
