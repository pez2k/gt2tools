namespace GT1.DataSplitter
{
    public class Computer : DataStructure
    {
        public Computer()
        {
            Header = "COMPUTE";
            Size = 0x20;
            // 0x10: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x10);
    }
}