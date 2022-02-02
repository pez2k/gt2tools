namespace GT2.DataSplitter
{
    using CarNameConversion;
    using StreamExtensions;

    public class ArcadeUnknown4 : DataStructure
    {
        public ArcadeUnknown4() => Size = 0x10;

        protected override string CreateOutputFilename() => Name + "\\" + rawData.ReadUInt().ToCarName() + ".dat";
    }
}