using System.IO;

namespace GT2.DataSplitter
{
    using StreamExtensions;

    public class ArcadeData : DataFile
    {
        private const int StringTableIndexPosition = 0x208; // why is it here, after a huge gap?

        public ArcadeData() : base((typeof(Brake), 0, false),
                                   (typeof(BrakeController), 1, false),
                                   (typeof(Steer), 2, false),
                                   (typeof(Chassis), 3, false),
                                   (typeof(Lightweight), 4, false),
                                   (typeof(RacingModify), 5, false),
                                   (typeof(Engine), 6, false), // We don't build unistrdb for Arcade, so not localised
                                   (typeof(PortPolish), 7, false),
                                   (typeof(EngineBalance), 8, false),
                                   (typeof(Displacement), 9, false),
                                   (typeof(Computer), 10, false),
                                   (typeof(NATune), 11, false),
                                   (typeof(TurbineKit), 12, false),
                                   (typeof(Drivetrain), 13, false),
                                   (typeof(Flywheel), 14, false),
                                   (typeof(Clutch), 14, false),
                                   (typeof(PropellerShaft), 15, false),
                                   (typeof(Gear), 16, false),
                                   (typeof(Suspension), 17, false),
                                   (typeof(Intercooler), 18, false),
                                   (typeof(Muffler), 19, false),
                                   (typeof(LSD), 20, false),
                                   (typeof(TiresFront), 24, false),
                                   (typeof(TiresRear), 25, false),
                                   (typeof(TireSize), 21, false),
                                   (typeof(TireCompoundArcade), 22, false),
                                   (typeof(TireForceVol), 23, false),
                                   (typeof(ActiveStabilityControl), 26, false),
                                   (typeof(TractionControlSystem), 27, false),
                                   (typeof(Wheel), 28, false),
                                   (typeof(Event), 30, false),
                                   (typeof(EnemyCarsArcade), 29, false),
                                   (typeof(CarArcadeRacing), 31, false),
                                   (typeof(CarArcadeDrift), 32, false))
        {
        }

        protected override void ReadDataFromFile(Stream file)
        {
            base.ReadDataFromFile(file);
            file.Position = StringTableIndexPosition;
            uint blockStart = file.ReadUInt();
            uint blockSize = file.ReadUInt(); // unused
            ASCIIStringTable.Read(file, blockStart);
        }

        protected override void WriteDataToFile(Stream file)
        {
            base.WriteDataToFile(file);
            ASCIIStringTable.Write(file, StringTableIndexPosition);
        }
    }
}