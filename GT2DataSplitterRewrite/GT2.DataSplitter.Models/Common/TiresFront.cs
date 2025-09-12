namespace GT2.DataSplitter.Models.Common
{
    public class TiresFront
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
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