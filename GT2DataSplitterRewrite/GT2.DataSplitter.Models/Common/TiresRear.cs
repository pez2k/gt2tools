namespace GT2.DataSplitter.Models.Common
{
    public class TiresRear
    {
        public string CarId { get; set; } = "";
        public byte Stage { get; set; }
        public byte SteeringReaction1 { get; set; }
        public byte WheelSize { get; set; }
        public byte SteeringReaction2 { get; set; }
        public byte TireCompound { get; set; }
        public byte TireForceVolMaybe { get; set; }
        public byte SlipMultiplier { get; set; }
        public byte GripMultiplier { get; set; }
    }
}