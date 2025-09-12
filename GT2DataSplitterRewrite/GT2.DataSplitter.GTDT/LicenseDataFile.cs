namespace GT2.DataSplitter.GTDT
{
    using Common;
    using LicenseData;
    using Models;

    public class LicenseDataFile : DataFileWithStrings<LicenseModel>
    {
        protected override int StringTableIndexPosition { get; } = 0x1F8; // why is it here, after a huge gap?

        public LicenseDataFile() : base(typeof(Brake),
                                        typeof(BrakeController),
                                        typeof(Steer),
                                        typeof(Chassis),
                                        typeof(Lightweight),
                                        typeof(RacingModify),
                                        typeof(Engine),
                                        typeof(PortPolish),
                                        typeof(EngineBalance),
                                        typeof(Displacement),
                                        typeof(Computer),
                                        typeof(NATune),
                                        typeof(TurbineKit),
                                        typeof(Drivetrain),
                                        typeof(Flywheel),
                                        typeof(Clutch),
                                        typeof(PropellerShaft),
                                        typeof(Gear),
                                        typeof(Suspension),
                                        typeof(Intercooler),
                                        typeof(Muffler),
                                        typeof(LSD),
                                        typeof(TiresFront),
                                        typeof(TiresRear),
                                        typeof(TireSize),
                                        typeof(TireCompoundLicense),
                                        typeof(TireForceVol),
                                        typeof(ActiveStabilityControl),
                                        typeof(TractionControlSystem),
                                        typeof(Wheel),
                                        typeof(EventLicense),
                                        typeof(CarLicense))
        {
        }
    }
}