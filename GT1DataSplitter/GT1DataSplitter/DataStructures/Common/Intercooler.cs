namespace GT1.DataSplitter
{
    public class Intercooler : DataStructure
    {
        public Intercooler()
        {
            Header = "INCOOL";
            Size = 0x20;
            // 0x10: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x10);
    }
}