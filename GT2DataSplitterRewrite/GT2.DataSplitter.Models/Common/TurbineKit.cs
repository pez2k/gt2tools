namespace GT2.DataSplitter.Models.Common
{
    public class TurbineKit
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte BoostGaugeLimit { get; set; }
        public byte LowRPMBoost { get; set; }
        public byte HighRPMBoost { get; set; }
        public byte SpoolRate { get; set; }
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }
        public sbyte RPMIncrease { get; set; }
        public sbyte RedlineIncrease { get; set; }
        public byte HighRPMPowerMultiplier { get; set; }
        public byte LowRPMPowerMultiplier { get; set; }
    }
}