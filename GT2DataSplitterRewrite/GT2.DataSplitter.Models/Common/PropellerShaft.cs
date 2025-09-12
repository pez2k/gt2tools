namespace GT2.DataSplitter.Models.Common
{
    public class PropellerShaft
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte RPMDropRate { get; set; }
        public byte Inertia { get; set; }
        public byte Inertia2 { get; set; }
    }
}