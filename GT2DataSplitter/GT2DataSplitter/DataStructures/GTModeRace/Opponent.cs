using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class Opponent : CsvDataStructure<OpponentData, OpponentCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return base.CreateOutputFilename(data).Replace(".csv", "_" + Data.CarId.ToCarName() + ".csv");
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x60
    public struct OpponentData
    {
        public uint CarId; // standard thing (0)
        public ushort Brakes; // (4)
        public ushort BrakeBalanceController;
        public ushort Steering;
        public ushort Dimensions; // (a)
        public ushort WeightReduction; // (c)
        public ushort Body;// (e)
        public ushort Engine; // (10)
        public ushort PortPolishing; // 12
        public ushort EngineBalancing; // 16
        public ushort DisplacementIncrease; // 18
        public ushort Chip; // 1a
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
        public ushort ASMLevel; // 34
        public ushort TCSLevel; // 36
        public ushort RimsCode3; // 38
        public ushort Unknown; // 0x3a
        public ushort Unknown2; // 0x3c
        public ushort Unknown3; // 0x3e
        public ushort Unknown4; // 0x40 - at least one byte of this seems to control colour
        public ushort Unknown5; // 0x42 - at least one byte of this seems to control colour
        public ushort Unknown6; // 0x44
        public ushort Unknown7; // 0x46
        public ushort Unknown8; // 0x48
        public ushort Unknown9; // 0x48
        public ushort Unknown10; // 0x4c
        public ushort Unknown11; // 0x4e
        public ushort Unknown12; // 0x50
        public ushort Unknown13; // 0x52
        public ushort Unknown14; // 0x54
        public ushort Unknown15; // 0x56
        public ushort Unknown16; // 0x58
        public ushort Unknown17; // 0x5a
        public byte PowerPercentage; // 0x5c
        public byte Unknown18; // 0x5d
        public ushort OpponentId; // 0x5e
    }

    public sealed class OpponentCSVMap : ClassMap<OpponentData>
    {
        public OpponentCSVMap()
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
            Map(m => m.ASMLevel);
            Map(m => m.TCSLevel);
            Map(m => m.RimsCode3);
            Map(m => m.Unknown);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.Unknown10);
            Map(m => m.Unknown11);
            Map(m => m.Unknown12);
            Map(m => m.Unknown13);
            Map(m => m.Unknown14);
            Map(m => m.Unknown15);
            Map(m => m.Unknown16);
            Map(m => m.Unknown17);
            Map(m => m.PowerPercentage);
            Map(m => m.Unknown18);
            Map(m => m.OpponentId);
        }
    }
}
