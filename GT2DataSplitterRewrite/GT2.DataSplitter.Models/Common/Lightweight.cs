namespace GT2.DataSplitter.Models.Common
{
    public class Lightweight
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public ushort Weight { get; set; }
        public byte Unknown { get; set; }
        public byte Stage { get; set; }
    }
}