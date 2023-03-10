namespace GT1.DataSplitter
{
    public class EngineBalancing : DataStructure
    {
        public EngineBalancing()
        {
            Header = "BALANCE";
            Size = 0x20;
            // 0x10: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x10);
    }
}