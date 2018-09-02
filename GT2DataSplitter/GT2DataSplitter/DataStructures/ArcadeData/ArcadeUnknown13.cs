using GT2.CarNameConversion;
using GT2.StreamExtensions;

namespace GT2.DataSplitter
{
    public class ArcadeUnknown13 : DataStructure
    {
        public ArcadeUnknown13()
        {
            Size = 0x3C;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + data.ReadUInt().ToCarName() + ".dat";
        }
    }
}
