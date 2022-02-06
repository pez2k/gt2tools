using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class CarArcadeRacing : CsvDataStructure<CarArcadeData, CarArcadeCSVMap>
    {
        protected override string CreateOutputFilename() => Name + "\\" + data.CarId.ToCarName() + ".csv";
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x3C
    public struct CarArcadeData
    {
        public uint CarId; // (0)
        public ushort Brake; // (4)
        public ushort BrakeController;
        public ushort Steer;
        public ushort Chassis; // (a)
        public ushort Lightweight; // (c)
        public ushort RacingModify;// (e)
        public ushort Engine; // (10)
        public ushort PortPolish; // 12
        public ushort EngineBalance; // 14
        public ushort Displacement; // 16
        public ushort Computer; // 18
        public ushort NATune; // 1a
        public ushort TurbineKit; // 1c
        public ushort Drivetrain; // 1e
        public ushort Flywheel; // 20
        public ushort Clutch; // 22
        public ushort PropellerShaft; // 24
        public ushort LSD; // 26
        public ushort Gear; // 28
        public ushort Suspension; // 2a
        public ushort Intercooler; // 2c
        public ushort Muffler; // 2e
        public ushort TiresFront; // 30
        public ushort TiresRear; // 32
        public ushort Unknown1;
        public ushort Unknown2;
        public ushort Unknown3;
        public ushort Unknown4;
    }

    public sealed class CarArcadeCSVMap : ClassMap<CarArcadeData>
    {
        public CarArcadeCSVMap()
        {
            Map(m => m.CarId).CarId();
            Map(m => m.Brake).PartFilename(nameof(Brake));
            Map(m => m.BrakeController).PartFilename(nameof(BrakeController));
            Map(m => m.Steer).PartFilename(nameof(Steer));
            Map(m => m.Chassis).PartFilename(nameof(Chassis));
            Map(m => m.Lightweight).PartFilename(nameof(Lightweight));
            Map(m => m.RacingModify).PartFilename(nameof(RacingModify));
            Map(m => m.Engine).PartFilename(nameof(Engine));
            Map(m => m.PortPolish).PartFilename(nameof(PortPolish));
            Map(m => m.EngineBalance).PartFilename(nameof(EngineBalance));
            Map(m => m.Displacement).PartFilename(nameof(Displacement));
            Map(m => m.Computer).PartFilename(nameof(Computer));
            Map(m => m.NATune).PartFilename(nameof(NATune));
            Map(m => m.TurbineKit).PartFilename(nameof(TurbineKit));
            Map(m => m.Drivetrain).PartFilename(nameof(Drivetrain));
            Map(m => m.Flywheel).PartFilename(nameof(Flywheel));
            Map(m => m.Clutch).PartFilename(nameof(Clutch));
            Map(m => m.PropellerShaft).PartFilename(nameof(PropellerShaft));
            Map(m => m.LSD).PartFilename(nameof(LSD));
            Map(m => m.Gear).PartFilename(nameof(Gear));
            Map(m => m.Suspension).PartFilename(nameof(Suspension));
            Map(m => m.Intercooler).PartFilename(nameof(Intercooler));
            Map(m => m.Muffler).PartFilename(nameof(Muffler));
            Map(m => m.TiresFront).PartFilename(nameof(TiresFront));
            Map(m => m.TiresRear).PartFilename(nameof(TiresRear));
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
        }
    }
}