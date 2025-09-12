namespace GT2.DataSplitter.Models.Common
{
    public class GenericEngineUpgrade
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public sbyte PowerbandScaling { get; set; }
        public sbyte RPMIncrease { get; set; }
        public byte PowerMultiplier { get; set; }
    }
}