namespace GT2.DataSplitter.Models.Common
{
    public class Clutch
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte RPMDropRate { get; set; }
        public byte InertiaDisengaged { get; set; }
        public byte InertiaEngaged { get; set; }
        public byte InertialWeight { get; set; }
        public byte InertiaBraking { get; set; }
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
    }
}