namespace GT2.DataSplitter.GTDT
{
    using ArcadeData;
    using Common;
    using Models;

    public class ArcadeDataFile : DataFileWithStrings<ArcadeModel>
    {
        protected override int StringTableIndexPosition { get; } = 0x208; // why is it here, after a huge gap?

        public ArcadeDataFile() : base(typeof(Brake),
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
                                       typeof(TireCompoundArcade),
                                       typeof(TireForceVol),
                                       typeof(ActiveStabilityControl),
                                       typeof(TractionControlSystem),
                                       typeof(Wheel),
                                       typeof(Event),
                                       typeof(EnemyCarsArcade),
                                       typeof(CarArcadeRacing),
                                       typeof(CarArcadeDrift))
        {
        }
    }
}