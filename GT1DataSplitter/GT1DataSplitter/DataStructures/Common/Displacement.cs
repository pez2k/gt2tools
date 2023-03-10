namespace GT1.DataSplitter
{
    public class Displacement : DataStructure
    {
        public Displacement()
        {
            Header = "DISPLAC";
            Size = 0x20;
            // 0x10: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x10);
    }
}