namespace GT2.DataSplitter.Models
{
    using Common;
    using License;

    public class LicenseModel
    {
        public Brake[] Brake { get; set; } = [];
        public BrakeController[] BrakeController { get; set; } = [];
        public Steer[] Steer { get; set; } = [];
        public Chassis[] Chassis { get; set; } = [];
        public Lightweight[] Lightweight { get; set; } = [];
        public RacingModify[] RacingModify { get; set; } = [];
        public Engine[] Engine { get; set; } = [];
        public PortPolish[] PortPolish { get; set; } = [];
        public EngineBalance[] EngineBalance { get; set; } = [];
        public Displacement[] Displacement { get; set; } = [];
        public Computer[] Computer { get; set; } = [];
        public NATune[] NATune { get; set; } = [];
        public TurbineKit[] TurbineKit { get; set; } = [];
        public Drivetrain[] Drivetrain { get; set; } = [];
        public Flywheel[] Flywheel { get; set; } = [];
        public Clutch[] Clutch { get; set; } = [];
        public PropellerShaft[] PropellerShaft { get; set; } = [];
        public Gear[] Gear { get; set; } = [];
        public Suspension[] Suspension { get; set; } = [];
        public Intercooler[] Intercooler { get; set; } = [];
        public Muffler[] Muffler { get; set; } = [];
        public LSD[] LSD { get; set; } = [];
        public TiresFront[] TiresFront { get; set; } = [];
        public TiresRear[] TiresRear { get; set; } = [];
        public TireSize[] TireSize { get; set; } = [];
        public TireCompoundLicense[] TireCompoundLicense { get; set; } = [];
        public TireForceVol[] TireForceVol { get; set; } = [];
        public ActiveStabilityControl[] ActiveStabilityControl { get; set; } = [];
        public TractionControlSystem[] TractionControlSystem { get; set; } = [];
        public Wheel[] Wheel { get; set; } = [];
        public EventLicense[] EventLicense { get; set; } = [];
        public CarLicense[] CarLicense { get; set; } = [];
    }
}