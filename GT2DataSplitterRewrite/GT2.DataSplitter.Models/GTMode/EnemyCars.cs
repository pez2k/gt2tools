namespace GT2.DataSplitter.Models.GTMode
{
    public class EnemyCars
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
        public ushort Propshaft { get; set; }
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
        public ushort FinalDriveRatio { get; set; }
        public byte GearAutoSetting { get; set; }
        public byte LSDInitialFront { get; set; }
        public byte LSDAccelFront { get; set; }
        public byte LSDDecelFront { get; set; }
        public byte LSDInitialRearAYCLevel { get; set; }
        public byte LSDAccelRear { get; set; }
        public byte LSDDecelRear { get; set; }
        public byte DownforceFront { get; set; }
        public byte DownforceRear { get; set; }
        public byte CamberFront { get; set; }
        public byte CamberRear { get; set; }
        public byte ToeFront { get; set; }
        public byte ToeRear { get; set; }
        public byte RideHeightFront { get; set; }
        public byte RideHeightRear { get; set; }
        public byte SpringRateFront { get; set; }
        public byte SpringRateRear { get; set; }
        public byte DamperBoundFront1 { get; set; }
        public byte DamperBoundFront2 { get; set; }
        public byte DamperReboundFront1 { get; set; }
        public byte DamperReboundFront2 { get; set; }
        public byte DamperBoundRear1 { get; set; }
        public byte DamperBoundRear2 { get; set; }
        public byte DamperReboundRear1 { get; set; }
        public byte DamperReboundRear2 { get; set; }
        public byte StabiliserFront { get; set; }
        public byte StabiliserRear { get; set; }
        public byte ASMLevel { get; set; }
        public byte TCSLevel { get; set; }
        public byte Unknown3 { get; set; }
        public ushort Unknown4 { get; set; }
        public ushort PowerMultiplier { get; set; }
        public ushort OpponentId { get; set; }
    }
}