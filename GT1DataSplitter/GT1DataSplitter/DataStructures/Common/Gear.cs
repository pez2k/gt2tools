namespace GT1.DataSplitter
{
    public class Gear : DataStructure
    {
        public Gear()
        {
            Header = "GEAR";
            Size = 0x44;
            // 0x32: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x32);
    }
}