namespace GT2.DataSplitter
{
    using CarNameConversion;
    using StreamExtensions;

    public class ArcadeUnknown5 : DataStructure
    {
        public ArcadeUnknown5() => Size = 0x10;

        protected override string CreateOutputFilename() => Name + "\\" + rawData.ReadUInt().ToCarName() + ".dat";
    }
}