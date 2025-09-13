using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.GTModeRace
{
    using CarNameConversion;

    public class EnemyCars : MappedDataStructure<EnemyCars.Data, Models.GTMode.EnemyCars>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x60
        public struct Data
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

        public override Models.GTMode.EnemyCars MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.GTMode.EnemyCars
            {
                CarId = data.CarId.ToCarName(),
                Brake = data.Brake,
                BrakeController = data.BrakeController,
                Steer = data.Steer,
                Chassis = data.Chassis,
                Lightweight = data.Lightweight,
                RacingModify = data.RacingModify,
                Engine = data.Engine,
                PortPolish = data.PortPolish,
                EngineBalance = data.EngineBalance,
                Displacement = data.Displacement,
                Computer = data.Computer,
                NATune = data.NATune,
                TurbineKit = data.TurbineKit,
                Drivetrain = data.Drivetrain,
                Flywheel = data.Flywheel,
                Clutch = data.Clutch,
                Propshaft = data.Propshaft,
                LSD = data.LSD,
                Gear = data.Gear,
                Suspension = data.Suspension,
                Intercooler = data.Intercooler,
                Muffler = data.Muffler,
                TiresFront = data.TiresFront,
                TiresRear = data.TiresRear,
                ActiveStabilityControl = data.ActiveStabilityControl,
                TractionControlSystem = data.TractionControlSystem,
                RimsCode3 = data.RimsCode3,
                FinalDriveRatio = data.FinalDriveRatio,
                GearAutoSetting = data.GearAutoSetting,
                LSDInitialFront = data.LSDInitialFront,
                LSDAccelFront = data.LSDAccelFront,
                LSDDecelFront = data.LSDDecelFront,
                LSDInitialRearAYCLevel = data.LSDInitialRearAYCLevel,
                LSDAccelRear = data.LSDAccelRear,
                LSDDecelRear = data.LSDDecelRear,
                DownforceFront = data.DownforceFront,
                DownforceRear = data.DownforceRear,
                CamberFront = data.CamberFront,
                CamberRear = data.CamberRear,
                ToeFront = data.ToeFront,
                ToeRear = data.ToeRear,
                RideHeightFront = data.RideHeightFront,
                RideHeightRear = data.RideHeightRear,
                SpringRateFront = data.SpringRateFront,
                SpringRateRear = data.SpringRateRear,
                DamperBoundFront1 = data.DamperBoundFront1,
                DamperBoundFront2 = data.DamperBoundFront2,
                DamperReboundFront1 = data.DamperReboundFront1,
                DamperReboundFront2 = data.DamperReboundFront2,
                DamperBoundRear1 = data.DamperBoundRear1,
                DamperBoundRear2 = data.DamperBoundRear2,
                DamperReboundRear1 = data.DamperReboundRear1,
                DamperReboundRear2 = data.DamperReboundRear2,
                StabiliserFront = data.StabiliserFront,
                StabiliserRear = data.StabiliserRear,
                ASMLevel = data.ASMLevel,
                TCSLevel = data.TCSLevel,
                Unknown3 = data.Unknown3,
                Unknown4 = data.Unknown4,
                PowerMultiplier = data.PowerMultiplier,
                OpponentId = data.OpponentId
            };
    }
}