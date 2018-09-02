using GT2.CarNameConversion;
using GT2.StreamExtensions;

namespace GT2.DataSplitter
{
    public class ArcadeUnknown1 : DataStructure
    {
        public ArcadeUnknown1()
        {
            Size = 0x18;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + data.ReadUInt().ToCarName() + ".dat";
        }
    }
}
