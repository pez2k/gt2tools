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
            public ushort Unknown0; // (c)
            public ushort Body;// (e)
            public ushort Engine; // (10)
            public ushort Unknown1; // 12
            public ushort Unknown2; // 14
            public ushort Unknown3; // 16
            public ushort Unknown4; // 18
            public ushort NATuning; // 1a
            public ushort TurboKit; // 1c
            public ushort Drivetrain; // 1e
            public ushort Flywheel; // 20
            public ushort Clutch; // 22
            public ushort Unknown5; // 24
            public ushort Differential; // 26
            public ushort Transmission; // 28
            public ushort Suspension; // 2a
            public ushort Unknown6; // 2c
            public ushort Unknown7; // 2e
            public ushort TyresFront; // 30
            public ushort TyresRear; // 32
            public ushort Unknown8; // 34
            public ushort Unknown9; // 36
            public ushort RimsCode3; // 38
            public ushort ManufacturerID; // 0x3a
            public ushort NameFirstPart; // 0x3c
            public ushort NameSecondPart; // 0x3e
            public byte IsSpecial2; // 0x40
            public byte Year; // 0x41 
            public ushort Unknown10; // 42
            public uint Price; // 0x44
        }
    }

    public sealed class CarCSVMap : ClassMap<Car.StructureData>
    {
        public CarCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Brakes).TypeConverter(Utils.GetFileNameConverter("Brakes"));
            Map(m => m.IsSpecial);
            Map(m => m.WeightReduction).TypeConverter(Utils.GetFileNameConverter("WeightReduction"));
            Map(m => m.Unknown0);
            Map(m => m.Body).TypeConverter(Utils.GetFileNameConverter("Body"));
            Map(m => m.Engine).TypeConverter(Utils.GetFileNameConverter("Engine"));
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.NATuning).TypeConverter(Utils.GetFileNameConverter("NATuning"));
            Map(m => m.TurboKit).TypeConverter(Utils.GetFileNameConverter("TurboKit"));
            Map(m => m.Drivetrain).TypeConverter(Utils.GetFileNameConverter("Drivetrain"));
            Map(m => m.Flywheel).TypeConverter(Utils.GetFileNameConverter("Flywheel"));
            Map(m => m.Clutch).TypeConverter(Utils.GetFileNameConverter("Clutch"));
            Map(m => m.Unknown5);
            Map(m => m.Differential).TypeConverter(Utils.GetFileNameConverter("Differential"));
            Map(m => m.Transmission).TypeConverter(Utils.GetFileNameConverter("Transmission"));
            Map(m => m.Suspension).TypeConverter(Utils.GetFileNameConverter("Suspension"));
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.TyresFront).TypeConverter(Utils.GetFileNameConverter("TyresFront"));
            Map(m => m.TyresRear).TypeConverter(Utils.GetFileNameConverter("TyresRear"));
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.RimsCode3);
            Map(m => m.ManufacturerID);
            Map(m => m.NameFirstPart);
            Map(m => m.NameSecondPart);
            Map(m => m.IsSpecial2);
            Map(m => m.Year);
            Map(m => m.Unknown10);
            Map(m => m.Price);
            Map(m => m.CarId);
        }
    }
}
