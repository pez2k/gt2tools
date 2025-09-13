using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.ArcadeData
{
    using CarNameConversion;

    public class CarArcadeRacing : MappedDataStructure<CarArcadeRacing.Data, Models.Arcade.CarArcadeRacing>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x3C
        public struct Data
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

        public override Models.Arcade.CarArcadeRacing MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Arcade.CarArcadeRacing
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
                Unknown1 = data.Unknown1,
                Unknown2 = data.Unknown2,
                Unknown3 = data.Unknown3,
                Unknown4 = data.Unknown4
            };
    }
}