using System.IO;
using System.Runtime.InteropServices;
using CsvHelper;
using System.Text;
using CsvHelper.Configuration;

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
            public ushort FrontTyres; // 30
            public ushort RearTyres; // 32
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
            Map(m => m.Brakes);
            Map(m => m.IsSpecial);
            Map(m => m.WeightReduction);
            Map(m => m.Unknown0);
            Map(m => m.Body);
            Map(m => m.Engine);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.NATuning);
            Map(m => m.TurboKit);
            Map(m => m.Drivetrain);
            Map(m => m.Flywheel);
            Map(m => m.Clutch);
            Map(m => m.Unknown5);
            Map(m => m.Differential);
            Map(m => m.Transmission);
            Map(m => m.Suspension);
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.FrontTyres);
            Map(m => m.RearTyres);
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.RimsCode3);
            Map(m => m.ManufacturerID);
            Map(m => m.NameFirstPart);
            Map(m => m.NameSecondPart);
            Map(m => m.Year);
            Map(m => m.Unknown10);
            Map(m => m.Price);
            Map(m => m.CarId);
        }
    }
}
