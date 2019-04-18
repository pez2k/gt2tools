using GT2.CarNameConversion;
using StreamExtensions;

namespace GT2.DataSplitter
{
    public class ArcadeUnknown5 : DataStructure
    {
        public ArcadeUnknown5()
        {
            Size = 0x10;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + data.ReadUInt().ToCarName() + ".dat";
        }
    }
}
