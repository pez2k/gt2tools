namespace GT2.DataSplitter.Models.GTMode
{
    public class Car
    {
        public string CarId { get; set; } = "";
        public ushort Brake { get; set; }
        public ushort BrakeController { get; set; }
        public ushort Steer { get; set; }
        public ushort Chassis { get; set; }
        public ushort Lightweight { get; set; }
        public ushort RacingModify { get; set; }
        public ushort Engine { get; set; }
        public ushort PortPolish { get; set; }
        public ushort EngineBalance { get; set; }
        public ushort Displacement { get; set; }
        public ushort Computer { get; set; }
        public ushort NATune { get; set; }
        public ushort TurbineKit { get; set; }
        public ushort Drivetrain { get; set; }
        public ushort Flywheel { get; set; }
        public ushort Clutch { get; set; }
        public ushort PropellerShaft { get; set; }
        public ushort LSD { get; set; }
        public ushort Gear { get; set; }
        public ushort Suspension { get; set; }
        public ushort Intercooler { get; set; }
        public ushort Muffler { get; set; }
        public ushort TiresFront { get; set; }
        public ushort TiresRear { get; set; }
        public ushort ActiveStabilityControl { get; set; }
        public ushort TractionControlSystem { get; set; }
        public ushort RimsCode3 { get; set; }
        public ushort ManufacturerID { get; set; }
        public string NameFirstPart { get; set; } = "";
        public string NameSecondPart { get; set; } = "";
        public bool HasAllTiresBought { get; set; }
        public byte Year { get; set; }
        public ushort Unknown { get; set; }
        public uint Price { get; set; }
    }
}