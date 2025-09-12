namespace GT2.DataSplitter.Models.Common
{
    public class Steer
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte Unknown1 { get; set; }
        public byte Angle1Speed { get; set; }
        public byte Angle2Speed { get; set; }
        public byte Angle3Speed { get; set; }
        public byte Angle4Speed { get; set; }
        public byte Angle5Speed { get; set; }
        public byte Angle6Speed { get; set; }
        public byte Angle1 { get; set; }
        public byte Angle2 { get; set; }
        public byte Angle3 { get; set; }
        public byte Angle4 { get; set; }
        public byte Angle5 { get; set; }
        public byte Angle6 { get; set; }
        public byte MaxSteeringAngle { get; set; }
        public byte Unknown2 { get; set; }
    }
}