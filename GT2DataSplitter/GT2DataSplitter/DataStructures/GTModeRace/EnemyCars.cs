using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class EnemyCars : CsvDataStructure<EnemyCarsData, EnemyCarsCSVMap>
    {
        public EnemyCars()
        {
            CacheFilename = true;
        }

        public override void Dump()
        {
            if (!FileNameCache.Cache.ContainsKey(Name))
            {
                FileNameCache.Add(Name, "None");
            }
            base.Dump();
        }

        public override void Import(string filename)
        {
            if (!FileNameCache.Cache.ContainsKey(Name))
            {
                FileNameCache.Add(Name, "None");
            }
            base.Import(filename);
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Data.OpponentId.ToString("D4") + "_" + Data.CarId.ToCarName() + ".csv";
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x60
    public struct EnemyCarsData
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
        public ushort Propshaft; // 24
        public ushort LSD; // 26
        public ushort Gear; // 28
        public ushort Suspension; // 2a
        public ushort Intercooler; // 2c
        public ushort Muffler; // 2e
        public ushort TiresFront; // 30
        public ushort TiresRear; // 32
        public ushort ActiveStabilityControl; // 34
        public ushort TractionControlSystem; // 36
        public ushort RimsCode3; // 38
        public ushort FinalDriveRatio; // 0x3a
        public byte GearAutoSetting; // 0x3c
        public byte LSDInitialFront;
        public byte LSDAccelFront; // 0x3e
        public byte LSDDecelFront;
        public byte LSDInitialRearAYCLevel; // 0x40 - at least one byte of this seems to control colour
        public byte LSDAccelRear;
        public byte LSDDecelRear; // 0x42 - at least one byte of this seems to control colour
        public byte DownforceFront;
        public byte DownforceRear; // 0x44
        public byte CamberFront;
        public byte CamberRear; // 0x46
        public byte ToeFront;
        public byte ToeRear; // 0x48
        public byte RideHeightFront;
        public byte RideHeightRear; // 0x48
        public byte SpringRateFront;
        public byte SpringRateRear; // 0x4c
        public byte DamperBoundFront1;
        public byte DamperBoundFront2; // 0x4e
        public byte DamperReboundFront1;
        public byte DamperReboundFront2; // 0x50
        public byte DamperBoundRear1;
        public byte DamperBoundRear2; // 0x52
        public byte DamperReboundRear1;
        public byte DamperReboundRear2; // 0x54
        public byte StabiliserFront;
        public byte StabiliserRear; // 0x56
        public byte ASMLevel;
        public byte TCSLevel; // 0x58
        public byte Unknown3;
        public ushort Unknown4; // 0x5a
        public ushort PowerMultiplier; // 0x5c
        public ushort OpponentId; // 0x5e
    }

    public sealed class EnemyCarsCSVMap : ClassMap<EnemyCarsData>
    {
        public EnemyCarsCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
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
            Map(m => m.Propshaft).PartFilename(nameof(PropellerShaft));
            Map(m => m.LSD).PartFilename(nameof(LSD));
            Map(m => m.Gear).PartFilename(nameof(Gear));
            Map(m => m.Suspension).PartFilename(nameof(Suspension));
            Map(m => m.Intercooler).PartFilename(nameof(Intercooler));
            Map(m => m.Muffler).PartFilename(nameof(Muffler));
            Map(m => m.TiresFront).PartFilename(nameof(TiresFront));
            Map(m => m.TiresRear).PartFilename(nameof(TiresRear));
            Map(m => m.ActiveStabilityControl).PartFilename(nameof(ActiveStabilityControl));
            Map(m => m.TractionControlSystem).PartFilename(nameof(TractionControlSystem));
            Map(m => m.RimsCode3);
            Map(m => m.FinalDriveRatio);
            Map(m => m.GearAutoSetting);
            Map(m => m.LSDInitialFront);
            Map(m => m.LSDAccelFront);
            Map(m => m.LSDDecelFront);
            Map(m => m.LSDInitialRearAYCLevel);
            Map(m => m.LSDAccelRear);
            Map(m => m.LSDDecelRear);
            Map(m => m.DownforceFront);
            Map(m => m.DownforceRear);
            Map(m => m.CamberFront);
            Map(m => m.CamberRear);
            Map(m => m.ToeFront);
            Map(m => m.ToeRear);
            Map(m => m.RideHeightFront);
            Map(m => m.RideHeightRear);
            Map(m => m.SpringRateFront);
            Map(m => m.SpringRateRear);
            Map(m => m.DamperBoundFront1);
            Map(m => m.DamperBoundFront2);
            Map(m => m.DamperReboundFront1);
            Map(m => m.DamperReboundFront2);
            Map(m => m.DamperBoundRear1);
            Map(m => m.DamperBoundRear2);
            Map(m => m.DamperReboundRear1);
            Map(m => m.DamperReboundRear2);
            Map(m => m.StabiliserFront);
            Map(m => m.StabiliserRear);
            Map(m => m.ASMLevel);
            Map(m => m.TCSLevel);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.PowerMultiplier);
            Map(m => m.OpponentId);
        }
    }
}