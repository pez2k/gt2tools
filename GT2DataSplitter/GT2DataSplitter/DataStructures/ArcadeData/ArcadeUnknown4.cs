using GT2.CarNameConversion;
using StreamExtensions;

namespace GT2.DataSplitter
{
    public class ArcadeUnknown4 : DataStructure
    {
        public ArcadeUnknown4()
        {
            Size = 0x10;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + data.ReadUInt().ToCarName() + ".dat";
        }
    }
}
