namespace GT2.DataSplitter.GTDT.Common
{
    public class ActiveStabilityControl : DataStructureWithModel<Models.Common.ActiveStabilityControl>
    {
        public ActiveStabilityControl() => Size = 0x10;

        public override Models.Common.ActiveStabilityControl MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.ActiveStabilityControl
            {
                Data = rawData
            };
    }
}