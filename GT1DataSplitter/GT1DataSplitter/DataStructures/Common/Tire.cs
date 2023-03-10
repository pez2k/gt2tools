namespace GT1.DataSplitter
{
    public class Tire : DataStructure
    {
        public Tire()
        {
            Header = "TIRE";
            Size = 0x14;
            // 0x0: compound ID?
            // 0x2: tyre size
            // 0x4: car ID this belongs to
            // 0x6: upgrade level maybe - second byte FF for stock / arcade, first byte FF for stock
            // 0x8: price
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }
}