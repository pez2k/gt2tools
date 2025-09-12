namespace GT2.DataSplitter.Models.Common
{
    public class Brake
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte BrakingPower { get; set; }
        public byte FrontBrakesUnknown { get; set; }
        public byte RearBrakesUnknown { get; set; }
    }
}