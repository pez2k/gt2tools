namespace GT1.DataSplitter
{
    public class Flywheel : DataStructure
    {
        public Flywheel()
        {
            Header = "FLYWHEL";
            Size = 0x14;
            // 0x4: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }
}