using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class Car : CsvDataStructure<CarData, CarCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Data.CarId.ToCarName() + ".csv";
        }

        public override void Read(FileStream infile)
        {
            base.Read(infile);
            CarNameStringTable.Add(Data.CarId.ToCarName(), StringTable.Get(Data.NameFirstPart), StringTable.Get(Data.NameSecondPart));
        }

        public override void Import(string filename)
        {
            base.Import(filename);
            var (nameFirstPart, nameSecondPart) = CarNameStringTable.Get(Data.CarId.ToCarName());
            CarData carData = Data;
            carData.NameFirstPart = StringTable.Add(nameFirstPart);
            carData.NameSecondPart = StringTable.Add(nameSecondPart);
            Data = carData;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x48
    public struct CarData
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
        public ushort ASMLevel; // 34
        public ushort TCSLevel; // 36
        public ushort RimsCode3; // 38
        public ushort ManufacturerID; // 0x3a
        public ushort NameFirstPart; // 0x3c
        public ushort NameSecondPart; // 0x3e
        public byte IsSpecial; // 0x40
        public byte Year; // 0x41 
        public ushort Unknown; // 42
        public uint Price; // 0x44
    }

    public sealed class CarCSVMap : ClassMap<CarData>
    {
        public CarCSVMap()
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
            Map(m => m.ManufacturerID);
            //Map(m => m.NameFirstPart).TypeConverter(StringTable.Lookup);
            //Map(m => m.NameSecondPart).TypeConverter(StringTable.Lookup);
            Map(m => m.IsSpecial);
            Map(m => m.Year);
            Map(m => m.Unknown);
            Map(m => m.Price);
        }
    }
}
