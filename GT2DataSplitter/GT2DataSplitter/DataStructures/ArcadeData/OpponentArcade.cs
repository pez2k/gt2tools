using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class OpponentArcade : CsvDataStructure<OpponentArcadeData, OpponentArcadeCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return base.CreateOutputFilename(data).Replace(".csv", "_" + Data.CarId.ToCarName() + ".csv");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x60
    public struct OpponentArcadeData
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
        public ushort Unknown; // 0x3a
        public ushort Unknown2; // 0x3c
        public ushort Unknown3; // 0x3e
        public byte Unknown4;
        public byte Unknown5;
        public byte Unknown6;
        public byte Unknown7;
        public ushort Unknown8;
        public byte Unknown9;
        public byte Unknown10;
        public byte Unknown11;
        public byte Unknown12;
        public byte Unknown13;
        public byte Unknown14;
        public byte Unknown15;
        public byte Unknown16;
        public byte Unknown17;
        public byte Unknown18;
        public byte Unknown19;
        public byte Unknown20;
        public byte Unknown21;
        public byte Unknown22;
        public byte Unknown23;
        public byte Unknown24;
        public byte Unknown25;
        public byte Unknown26;
        public byte Unknown27;
        public byte Unknown28;
        public byte Unknown29;
        public byte Unknown30;
        public byte Unknown31;
        public byte Unknown32;
        public byte Unknown33;
        public byte Unknown34;
        public byte Unknown35;
        public byte Unknown36;
        public byte PowerPercentage;
        public byte Unknown37;
        public ushort OpponentId;
    }

    public sealed class OpponentArcadeCSVMap : ClassMap<OpponentArcadeData>
    {
        public OpponentArcadeCSVMap()
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
            Map(m => m.Unknown18);
            Map(m => m.Unknown19);
            Map(m => m.Unknown20);
            Map(m => m.Unknown21);
            Map(m => m.Unknown22);
            Map(m => m.Unknown23);
            Map(m => m.Unknown24);
            Map(m => m.Unknown25);
            Map(m => m.Unknown26);
            Map(m => m.Unknown27);
            Map(m => m.Unknown28);
            Map(m => m.Unknown29);
            Map(m => m.Unknown30);
            Map(m => m.Unknown31);
            Map(m => m.Unknown32);
            Map(m => m.Unknown33);
            Map(m => m.Unknown34);
            Map(m => m.Unknown35);
            Map(m => m.Unknown36);
            Map(m => m.PowerPercentage);
            Map(m => m.Unknown37);
            Map(m => m.OpponentId);
        }
    }
}
