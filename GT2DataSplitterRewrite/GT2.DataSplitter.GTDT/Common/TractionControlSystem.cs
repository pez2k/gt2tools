namespace GT2.DataSplitter.GTDT.Common
{
    public class TractionControlSystem : DataStructure
    {
        public TractionControlSystem() => Size = 0x10;

        public Models.Common.TractionControlSystem MapToModel() =>
            new Models.Common.TractionControlSystem
            {
                Data = rawData
            };
    }
}