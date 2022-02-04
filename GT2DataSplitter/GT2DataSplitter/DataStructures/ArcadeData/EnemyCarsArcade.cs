using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class EnemyCarsArcade : CsvDataStructure<EnemyCarsArcadeData, EnemyCarsArcadeCSVMap>
    {
        public EnemyCarsArcade()
        {
            cacheFilename = true;
            filenameCacheNameOverride = nameof(EnemyCars);
        }

        public override void Dump()
        {
            if (!FileNameCache.Cache.ContainsKey(filenameCacheNameOverride))
            {
                FileNameCache.Add(filenameCacheNameOverride, "None");
            }
            base.Dump();
        }

        public override void Import(string filename)
        {
            if (!FileNameCache.Cache.ContainsKey(filenameCacheNameOverride))
            {
                FileNameCache.Add(filenameCacheNameOverride, "None");
            }
            base.Import(filename);
        }

        protected override string CreateOutputFilename() => Name + "\\" + data.OpponentId.ToString("D4") + "_" + data.CarId.ToCarName() + ".csv";
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x60
    public struct EnemyCarsArcadeData
    {
        public uint CarId; // standard thing (0)
        public ushort Brake; // (4)
        public ushort BrakeController;
        public ushort Steer;
        public ushort Chassis; // (a)
        public ushort Lightweight; // (c)
        public ushort RacingModify;// (e)
        public ushort Engine; // (10)
        public ushort PortPolish; // 12
        public ushort EngineBalance; // 16
        public ushort Displacement; // 18
        public ushort Computer; // 1a
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

    public sealed class EnemyCarsArcadeCSVMap : ClassMap<EnemyCarsArcadeData>
    {
        public EnemyCarsArcadeCSVMap()
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