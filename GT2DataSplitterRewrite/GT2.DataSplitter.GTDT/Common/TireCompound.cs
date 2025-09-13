namespace GT2.DataSplitter.GTDT.Common
{
    public class TireCompound : DataStructureWithModel<Models.Common.TireCompound>
    {
        public TireCompound() => Size = 0x40;

        public override Models.Common.TireCompound MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.TireCompound
            {
                Data = rawData
            };
    }
}