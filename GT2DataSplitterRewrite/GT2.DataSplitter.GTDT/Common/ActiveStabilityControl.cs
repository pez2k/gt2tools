namespace GT2.DataSplitter.GTDT.Common
{
    public class ActiveStabilityControl : DataStructure
    {
        public ActiveStabilityControl() => Size = 0x10;

        public Models.Common.ActiveStabilityControl MapToModel() =>
            new Models.Common.ActiveStabilityControl
            {
                Data = rawData
            };
    }
}