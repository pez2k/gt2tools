namespace GT1.DataSplitter
{
    public abstract class Stabilizer : DataStructure
    {
        public Stabilizer()
        {
            Header = "STABILZ";
            Size = 0x14;
            // 0x4: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }
}