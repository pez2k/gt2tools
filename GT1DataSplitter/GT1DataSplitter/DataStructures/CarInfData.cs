namespace GT1.DataSplitter
{
    public class CarInfData : DataFile
    {
        public CarInfData() : base((typeof(CarGT), 25, false),
                                   (typeof(CarArcadeGrip), 26, false),
                                   (typeof(CarArcadeDrift), 27, false),
                                   (typeof(EnemyCarsSpotRace), 28, false),
                                   (typeof(EnemyCarsDrivetrain), 29, false),
                                   (typeof(EnemyCarsCountry), 30, false),
                                   (typeof(EnemyCarsNormal), 31, false),
                                   (typeof(EnemyCarsHardTuned), 32, false),
                                   (typeof(EnemyCarsMegaspeed), 33, false),
                                   (typeof(EnemyCarsSunday), 34, false),
                                   (typeof(EnemyCarsClubman), 35, false),
                                   (typeof(EnemyCarsGTCup), 36, false),
                                   (typeof(EnemyCarsWorldCup), 37, false),
                                   (typeof(Spec), 38, false),
                                   (typeof(Adjust), 1, false),
                                   (typeof(EngineBalancing), 2, false),
                                   (typeof(Brake), 3, false),
                                   (typeof(BrakeController), 4, false),
                                   (typeof(Clutch), 5, false),
                                   (typeof(Compressor), 6, false),
                                   (typeof(Computer), 7, false),
                                   (typeof(Displacement), 8, false),
                                   (typeof(Flywheel), 9, false),
                                   (typeof(Gear), 10, false),
                                   (typeof(Intercooler), 11, false),
                                   (typeof(Lightweight), 12, false),
                                   (typeof(Muffler), 13, false),
                                   (typeof(NATune), 14, false),
                                   (typeof(PortPolish), 15, false),
                                   (typeof(Propshaft), 16, false),
                                   (typeof(RacingModify), 17, false),
                                   (typeof(StabilizerFront), 18, false),
                                   (typeof(StabilizerRear), 19, false),
                                   (typeof(Suspension), 20, false),
                                   (typeof(Tire), 24, false),
                                   (typeof(TurbineKit), 21, false),
                                   (typeof(TireCompound), 22, false),
                                   (typeof(TireSize), 23, false),
                                   (typeof(CarColors), 38, false))
        {
        }
    }
}