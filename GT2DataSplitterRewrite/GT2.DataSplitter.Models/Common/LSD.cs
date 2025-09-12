namespace GT2.DataSplitter.Models.Common
{
    public class LSD
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte Unknown { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }
        public byte FrontUnknown { get; set; }
        public byte DefaultInitialFront { get; set; }
        public byte MinInitialFront { get; set; }
        public byte MaxInitialFront { get; set; }
        public byte DefaultAccelFront { get; set; }
        public byte MinAccelFront { get; set; }
        public byte MaxAccelFront { get; set; }
        public byte DefaultDecelFront { get; set; }
        public byte MinDecelFront { get; set; }
        public byte MaxDecelFront { get; set; }
        public byte RearUnknown { get; set; }
        public byte DefaultInitialRear { get; set; }
        public byte MinInitialRear { get; set; }
        public byte MaxInitialRear { get; set; }
        public byte DefaultAccelRear { get; set; }
        public byte MinAccelRear { get; set; }
        public byte MaxAccelRear { get; set; }
        public byte DefaultDecelRear { get; set; }
        public byte MinDecelRear { get; set; }
        public byte MaxDecelRear { get; set; }
    }
}