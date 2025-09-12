namespace GT2.DataSplitter.Models.Common
{
    public class Flywheel
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte RPMDropRate { get; set; }
        public byte ShiftDelay { get; set; }
        public byte InertialWeight { get; set; }
    }
}