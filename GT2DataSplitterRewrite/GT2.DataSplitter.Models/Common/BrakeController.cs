namespace GT2.DataSplitter.Models.Common
{
    public class BrakeController
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte MaxFrontBias { get; set; }
        public byte Unknown { get; set; }
        public byte Unknown2 { get; set; }
        public byte DefaultBias { get; set; }
        public byte MaxRearBias { get; set; }
        public byte Unknown3 { get; set; }
        public byte Unknown4 { get; set; }
    }
}