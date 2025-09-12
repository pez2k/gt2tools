namespace GT2.DataSplitter.GTDT
{
    using Common;
    using Models;
    using GTModeData;

    public class GTModeDataFile : DataFile<GTModeModel>
    {
        public GTModeDataFile() : base(typeof(Brake),
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
                                       typeof(TireCompound),
                                       typeof(TireForceVol),
                                       typeof(ActiveStabilityControl),
                                       typeof(TractionControlSystem),
                                       typeof(Wheel),
                                       typeof(Car))
        {
        }
    }
}