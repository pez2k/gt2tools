namespace GT2.DataSplitter.Models.Common
{
    public class RacingModify
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public uint BodyId { get; set; }
        public byte Weight { get; set; }
        public byte BodyRollAmount { get; set; }
        public byte Stage { get; set; }
        public byte Drag { get; set; }
        public byte FrontDownforceMinimum { get; set; }
        public byte FrontDownforceMaximum { get; set; }
        public byte FrontDownforceDefault { get; set; }
        public byte RearDownforceMinimum { get; set; }
        public byte RearDownforceMaximum { get; set; }
        public byte RearDownforceDefault { get; set; }
        public byte Unknown3 { get; set; }
        public byte Unknown4 { get; set; }
        public byte Unknown5 { get; set; }
        public byte Unknown6 { get; set; }
        public ushort Width { get; set; }
    }
}