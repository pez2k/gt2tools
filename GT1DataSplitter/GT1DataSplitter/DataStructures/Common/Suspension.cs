namespace GT1.DataSplitter
{
    public class Suspension : DataStructure
    {
        public Suspension()
        {
            Header = "SUSPENS";
            Size = 0x48;
            // 0x36: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x36);
    }
}