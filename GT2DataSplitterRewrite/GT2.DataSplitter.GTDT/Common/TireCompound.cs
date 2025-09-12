namespace GT2.DataSplitter.GTDT.Common
{
    public class TireCompound : DataStructure
    {
        public TireCompound() => Size = 0x40;

        public virtual Models.Common.TireCompound MapToModel() =>
            new Models.Common.TireCompound
            {
                Data = rawData
            };
    }
}