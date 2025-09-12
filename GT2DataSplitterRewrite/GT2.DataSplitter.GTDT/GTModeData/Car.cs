using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.GTModeData
{
    using CarNameConversion;

    public class Car : MappedDataStructure<Car.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x48
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
            public ushort ActiveStabilityControl; // 34
            public ushort TractionControlSystem; // 36 ------------------ 0 for road cars, 1 for race?
            public ushort RimsCode3; // 38
            public ushort ManufacturerID; // 0x3a
            public ushort NameFirstPart; // 0x3c
            public ushort NameSecondPart; // 0x3e
            public byte HasAllTiresBought; // 0x40
            public byte Year; // 0x41 
            public ushort Unknown; // 42
            public uint Price; // 0x44
        }

        public Models.GTMode.Car MapToModel(UnicodeStringTable strings) =>
            new Models.GTMode.Car
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
                ActiveStabilityControl = data.ActiveStabilityControl,
                TractionControlSystem = data.TractionControlSystem,
                RimsCode3 = data.RimsCode3,
                ManufacturerID = data.ManufacturerID,
                NameFirstPart = strings.Get(data.NameFirstPart),
                NameSecondPart = strings.Get(data.NameSecondPart),
                HasAllTiresBought = data.HasAllTiresBought == 1,
                Year = data.Year,
                Unknown = data.Unknown,
                Price = data.Price
            };
    }
}