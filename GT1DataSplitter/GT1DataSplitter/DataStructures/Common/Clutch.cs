namespace GT1.DataSplitter
{
    public class Clutch : DataStructure
    {
        public Clutch()
        {
            Header = "CLUTCH";
            Size = 0x18;
            // 0x6: car ID ----- this is a common structure
            // 0x8: stage
            // 0x9: buyable? 00 normally, FF for Arcade parts?
            // 0xA: price - aligned to 4b
            // 0x10: name part 1
            // 0x14: name part 2
            // 0x16: unknown, always 01? ----- end common
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x6);
    }
}