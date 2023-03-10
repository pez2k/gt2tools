namespace GT1.DataSplitter
{
    public class CarInfData : DataFile
    {
        public CarInfData() : base((typeof(CarGT), 0, false),
                                   (typeof(CarArcadeGrip), 1, false),
                                   (typeof(CarArcadeDrift), 2, false),
                                   (typeof(EnemyCarsSpotRace), 3, false),
                                   (typeof(EnemyCarsDrivetrain), 4, false),
                                   (typeof(EnemyCarsCountry), 5, false),
                                   (typeof(EnemyCarsNormal), 6, false),
                                   (typeof(EnemyCarsHardTuned), 7, false),
                                   (typeof(EnemyCarsMegaspeed), 8, false),
                                   (typeof(EnemyCarsSunday), 9, false),
                                   (typeof(EnemyCarsClubman), 10, false),
                                   (typeof(EnemyCarsGTCup), 11, false),
                                   (typeof(EnemyCarsWorldCup), 12, false),
                                   (typeof(Spec), 13, false),
                                   (typeof(Adjust), 14, false),
                                   (typeof(EngineBalancing), 15, false),
                                   (typeof(Brake), 16, false),
                                   (typeof(BrakeController), 17, false),
                                   (typeof(Clutch), 18, false),
                                   (typeof(Compression), 19, false),
                                   (typeof(Computer), 20, false),
                                   (typeof(Displacement), 21, false),
                                   (typeof(Flywheel), 22, false),
                                   (typeof(Gear), 23, false),
                                   (typeof(Intercooler), 24, false),
                                   (typeof(Lightweight), 25, false),
                                   (typeof(Muffler), 26, false),
                                   (typeof(NATune), 27, false),
                                   (typeof(PortPolish), 28, false),
                                   (typeof(Propshaft), 29, false),
                                   (typeof(RacingModify), 30, false),
                                   (typeof(StabilizerFront), 31, false),
                                   (typeof(StabilizerRear), 32, false),
                                   (typeof(Suspension), 33, false),
                                   (typeof(Tire), 34, false),
                                   (typeof(TurbineKit), 35, false),
                                   (typeof(TireCompound), 36, false),
                                   (typeof(TireSize), 37, false),
                                   (typeof(CarColors), 38, false))
        {
        }
    }
}