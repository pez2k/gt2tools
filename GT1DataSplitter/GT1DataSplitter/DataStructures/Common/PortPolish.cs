namespace GT1.DataSplitter
{
    public class PortPolish : DataStructure
    {
        public PortPolish()
        {
            Header = "POLISH";
            Size = 0x20;
            // 0x10: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x10);
    }
}