namespace GT1.DataSplitter
{
    public class Brake : DataStructure
    {
        public Brake()
        {
            Header = "BRAKE";
            Size = 0x18;
            // 0x4: car ID
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }
}