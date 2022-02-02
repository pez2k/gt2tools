using System.IO;

namespace GT2.DataSplitter
{
    using StreamExtensions;

    public class ArcadeData : DataFile
    {
        private const int StringTableIndexPosition = 0x208; // why is it here, after a huge gap?

        public ArcadeData() : base((typeof(Brake), 0),
                                   (typeof(BrakeController), 1),
                                   (typeof(Steer), 2),
                                   (typeof(Chassis), 3),
                                   (typeof(Lightweight), 4),
                                   (typeof(RacingModify), 5),
                                   (typeof(Engine), 6),
                                   (typeof(PortPolish), 7),
                                   (typeof(EngineBalance), 8),
                                   (typeof(Displacement), 9),
                                   (typeof(Computer), 10),
                                   (typeof(NATune), 11),
                                   (typeof(TurbineKit), 12),
                                   (typeof(Drivetrain), 13),
                                   (typeof(Flywheel), 14),
                                   (typeof(Clutch), 14),
                                   (typeof(PropellerShaft), 15),
                                   (typeof(Gear), 16),
                                   (typeof(Suspension), 17),
                                   (typeof(Intercooler), 18),
                                   (typeof(Muffler), 19),
                                   (typeof(LSD), 20),
                                   (typeof(TiresFront), 21),
                                   (typeof(TiresRear), 22),
                                   (typeof(ArcadeUnknown1), 23),
                                   (typeof(ArcadeUnknown2), 24),
                                   (typeof(ArcadeUnknown3), 25),
                                   (typeof(ArcadeUnknown4), 26),
                                   (typeof(ArcadeUnknown5), 27),
                                   (typeof(ArcadeUnknown6), 28),
                                   (typeof(Event), 29),
                                   (typeof(EnemyCarsArcade), 30),
                                   (typeof(CarArcade), 31),
                                   (typeof(CarArcadeSports), 32))
        {
        }

        protected override void ReadDataFromFile(Stream file)
        {
            base.ReadDataFromFile(file);
            file.Position = StringTableIndexPosition;
            uint blockStart = file.ReadUInt();
            uint blockSize = file.ReadUInt(); // unused
            RaceStringTable.Read(file, blockStart);
        }

        protected override void WriteDataToFile(Stream file)
        {
            base.WriteDataToFile(file);
            RaceStringTable.Write(file, StringTableIndexPosition);
        }
    }
}