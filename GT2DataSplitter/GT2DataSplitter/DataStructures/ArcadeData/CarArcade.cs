using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class CarArcade : CsvDataStructure<CarArcadeData, CarArcadeCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Data.CarId.ToCarName() + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x3C
    public struct CarArcadeData
    {
        public uint CarId; // (0)
        public ushort Brakes; // (4)
        public ushort BrakeBalanceController;
        public ushort Steering;
        public ushort Dimensions; // (a)
        public ushort WeightReduction; // (c)
        public ushort Body;// (e)
        public ushort Engine; // (10)
        public ushort PortPolishing; // 12
        public ushort EngineBalancing; // 14
        public ushort DisplacementIncrease; // 16
        public ushort Chip; // 18
        public ushort NATuning; // 1a
        public ushort TurboKit; // 1c
        public ushort Drivetrain; // 1e
        public ushort Flywheel; // 20
        public ushort Clutch; // 22
        public ushort Propshaft; // 24
        public ushort Differential; // 26
        public ushort Transmission; // 28
        public ushort Suspension; // 2a
        public ushort Intercooler; // 2c
        public ushort Exhaust; // 2e
        public ushort TyresFront; // 30
        public ushort TyresRear; // 32
        public ushort Unknown1;
        public ushort Unknown2;
        public ushort Unknown3;
        public ushort Unknown4;
    }

    public sealed class CarArcadeCSVMap : ClassMap<CarArcadeData>
    {
        public CarArcadeCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Brakes).PartFilename("Brakes");
            Map(m => m.BrakeBalanceController).PartFilename("BrakeBalanceController");
            Map(m => m.Steering).PartFilename("Steering");
            Map(m => m.Dimensions).PartFilename("Dimensions");
            Map(m => m.WeightReduction).PartFilename("WeightReduction");
            Map(m => m.Body).PartFilename("Body");
            Map(m => m.Engine).PartFilename("Engine");
            Map(m => m.PortPolishing).PartFilename("PortPolishing");
            Map(m => m.EngineBalancing).PartFilename("EngineBalancing");
            Map(m => m.DisplacementIncrease).PartFilename("DisplacementIncrease");
            Map(m => m.Chip).PartFilename("Chip");
            Map(m => m.NATuning).PartFilename("NATuning");
            Map(m => m.TurboKit).PartFilename("TurboKit");
            Map(m => m.Drivetrain).PartFilename("Drivetrain");
            Map(m => m.Flywheel).PartFilename("Flywheel");
            Map(m => m.Clutch).PartFilename("Clutch");
            Map(m => m.Propshaft).PartFilename("Propshaft");
            Map(m => m.Differential).PartFilename("Differential");
            Map(m => m.Transmission).PartFilename("Transmission");
            Map(m => m.Suspension).PartFilename("Suspension");
            Map(m => m.Intercooler).PartFilename("Intercooler");
            Map(m => m.Exhaust).PartFilename("Exhaust");
            Map(m => m.TyresFront).PartFilename("TyresFront");
            Map(m => m.TyresRear).PartFilename("TyresRear");
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
        }
    }
}