namespace GT2.DataSplitter
{
    using CarNameConversion;
    using StreamExtensions;

    public class LicenseCar : DataStructure
    {
        public LicenseCar()
        {
            Size = 0x60;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + data.ReadUInt().ToCarName() + ".dat";
        }
    }
}
