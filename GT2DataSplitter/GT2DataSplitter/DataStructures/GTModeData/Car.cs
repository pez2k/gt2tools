using System.IO;
using System.Runtime.InteropServices;
using CsvHelper;
using System.Text;
using CsvHelper.Configuration;
using System;

namespace GT2DataSplitter
{
    public class Car : CarDataStructure
    {
        public Car()
        {
            Size = 0x48;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Utils.GetCarName(data.ReadUInt()) + ".csv";
        }

        public override void Read(FileStream infile)
        {
            base.Read(infile);

            GCHandle handle = GCHandle.Alloc(RawData, GCHandleType.Pinned);
            Data = (StructureData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(StructureData));
            handle.Free();
        }

        public override void Dump()
        {
            using (TextWriter output = new StreamWriter(File.Create(CreateOutputFilename(RawData)), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(output))
                {
                    csv.Configuration.RegisterClassMap<CarCSVMap>();
                    csv.Configuration.QuoteAllFields = true;
                    csv.WriteHeader<StructureData>();
                    csv.NextRecord();
                    csv.WriteRecord(Data);
                }
            }
        }

        public override void Import(string filename)
        {
            using (TextReader input = new StreamReader(filename, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(input))
                {
                    csv.Configuration.RegisterClassMap<CarCSVMap>();
                    csv.Read();
                    Data = csv.GetRecord<StructureData>();
                }
            }
        }

        public override void Write(FileStream outfile)
        {
            int size = Marshal.SizeOf(Data);
            RawData = new byte[size];

            IntPtr objectPointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(Data, objectPointer, true);
            Marshal.Copy(objectPointer, RawData, 0, size);
            Marshal.FreeHGlobal(objectPointer);

            base.Write(outfile);
        }

        public StructureData Data { get; set; }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct StructureData
        {
            public uint CarId; // (0)
            public ushort Brakes; // (4)
            public uint IsSpecial;
            public ushort WeightReduction; // (a)
            public ushort Unknown1; // (c)
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
            public byte IsSpecial2; // 0x40
            public byte Year; // 0x41 
            public ushort Unknown2; // 42
            public uint Price; // 0x44
        }
    }

    public sealed class CarCSVMap : ClassMap<Car.StructureData>
    {
        public CarCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Brakes).PartFilename("Brakes");
            Map(m => m.IsSpecial);
            Map(m => m.WeightReduction).PartFilename("WeightReduction");
            Map(m => m.Unknown1);
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
            Map(m => m.NameFirstPart);
            Map(m => m.NameSecondPart);
            Map(m => m.IsSpecial2);
            Map(m => m.Year);
            Map(m => m.Unknown2);
            Map(m => m.Price);
        }
    }
}
