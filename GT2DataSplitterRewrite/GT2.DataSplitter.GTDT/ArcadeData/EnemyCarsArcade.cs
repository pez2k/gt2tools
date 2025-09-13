using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.ArcadeData
{
    using CarNameConversion;

    public class EnemyCarsArcade : MappedDataStructure<EnemyCarsArcade.Data, Models.Arcade.EnemyCarsArcade>
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

        public override Models.Arcade.EnemyCarsArcade MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Arcade.EnemyCarsArcade
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
                PropellerShaft = data.PropellerShaft,
                LSD = data.LSD,
                Gear = data.Gear,
                Suspension = data.Suspension,
                Intercooler = data.Intercooler,
                Muffler = data.Muffler,
                TiresFront = data.TiresFront,
                TiresRear = data.TiresRear,
                Unknown = data.Unknown,
                Unknown2 = data.Unknown2,
                Unknown3 = data.Unknown3,
                Unknown4 = data.Unknown4,
                Unknown5 = data.Unknown5,
                Unknown6 = data.Unknown6,
                Unknown7 = data.Unknown7,
                Unknown8 = data.Unknown8,
                Unknown9 = data.Unknown9,
                Unknown10 = data.Unknown10,
                Unknown11 = data.Unknown11,
                Unknown12 = data.Unknown12,
                Unknown13 = data.Unknown13,
                Unknown14 = data.Unknown14,
                Unknown15 = data.Unknown15,
                Unknown16 = data.Unknown16,
                Unknown17 = data.Unknown17,
                Unknown18 = data.Unknown18,
                Unknown19 = data.Unknown19,
                Unknown20 = data.Unknown20,
                Unknown21 = data.Unknown21,
                Unknown22 = data.Unknown22,
                Unknown23 = data.Unknown23,
                Unknown24 = data.Unknown24,
                Unknown25 = data.Unknown25,
                Unknown26 = data.Unknown26,
                Unknown27 = data.Unknown27,
                Unknown28 = data.Unknown28,
                Unknown29 = data.Unknown29,
                Unknown30 = data.Unknown30,
                Unknown31 = data.Unknown31,
                Unknown32 = data.Unknown32,
                Unknown33 = data.Unknown33,
                Unknown34 = data.Unknown34,
                Unknown35 = data.Unknown35,
                Unknown36 = data.Unknown36,
                PowerPercentage = data.PowerPercentage,
                Unknown37 = data.Unknown37,
                OpponentId = data.OpponentId
            };
    }
}