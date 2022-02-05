namespace GT2.DataSplitter
{
    using CarNameConversion;
    using StreamExtensions;

    public class LicenseCar : DataStructure
    {
        public LicenseCar() => Size = 0x60;

        protected override string CreateOutputFilename() => $"{Name}\\{rawData[0x5E]:D2}_{rawData.ReadUInt().ToCarName()}.dat";
    }
}