namespace GT1.DataSplitter
{
    public class NATune : DataStructure
    {
        public NATune()
        {
            Header = "NATUNE";
            Size = 0x24;
            // 0x10: car ID
            // 0x12: stage - 01 00 for stg1, 03 01 stg2, 05 02 stg3
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x10);
    }
}