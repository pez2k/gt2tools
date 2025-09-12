namespace GT2.DataSplitter.Models.Common
{
    public class Chassis
    {
        public string CarId { get; set; } = "";
        public byte FrontWeightDistribution { get; set; }
        public byte Unknown2 { get; set; }
        public byte FrontGrip { get; set; }
        public byte RearGrip { get; set; }
        public ushort Length { get; set; }
        public ushort Height { get; set; }
        public ushort Wheelbase { get; set; }
        public ushort Weight { get; set; }
        public byte TurningResistance { get; set; }
        public byte PitchResistance { get; set; }
        public byte RollResistance { get; set; }
        public byte Unknown8 { get; set; }
    }
}