namespace GT2.DataSplitter.GTDT.Common
{
    public class TractionControlSystem : DataStructureWithModel<Models.Common.TractionControlSystem>
    {
        public TractionControlSystem() => Size = 0x10;

        public override Models.Common.TractionControlSystem MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.TractionControlSystem
            {
                Data = rawData
            };
    }
}