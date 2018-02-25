using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GT2DataSplitter
{
    public class Opponent : DataStructure
    {
        public Opponent()
        {
            Size = 0x60;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return base.CreateOutputFilename(data).Replace(".dat", ".csv");
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
                    csv.Configuration.RegisterClassMap<OpponentCSVMap>();
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
                    csv.Configuration.RegisterClassMap<OpponentCSVMap>();
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
            public uint CarId; // standard thing (0)
            public ushort Brakes; // (4)
            public uint IsSpecial; // 1 for special cars, 0 for not
            public ushort WeightReduction; // (a)
            public ushort Unknown0; // (c)
            public ushort Body;// (e)
            public ushort Engine; // (10)
            public ushort Unknown1; // 12
            public uint Unknown2; // 16
            public ushort Unknown2b; // 1a
            public ushort NATuning; // 1a -------- check!
            public ushort TurboKit; // 1c
            public ushort Drivetrain; // 1e
            public ushort Flywheel; // 20
            public ushort Clutch; // 22
            public ushort Unknown3; // 24
            public ushort Differential; // 26
            public ushort Transmission; // 28
            public ushort Suspension; // 2a
            public uint Unknown4; // 2c
            public ushort TyresFront; // 30
            public ushort TyresRear; // 32
            public uint Unknown5; // 34
            public ushort RimsCode3; // 38
            public ushort Unknown6; // 0x3a
            public ushort Unknown7; // 0x3c
            public ushort Unknown8; // 0x3e
            public uint Unknown9; // 0x40 - at least one byte of this seems to control colour
            public ushort Unknown10; // 0x44
            public ushort Unknown11; // 0x46
            public uint Unknown12; // 0x48
            public ushort Unknown13; // 0x4c
            public ushort Unknown14; // 0x4e
            public ushort Unknown15; // 0x50
            public ushort Unknown16; // 0x52
            public ushort Unknown17; // 0x54
            public ushort Unknown18; // 0x56
            public ushort Unknown19; // 0x58
            public ushort Unknown20; // 0x5a
            public byte PowerPercentage; // 0x5c
            public byte Unknown21; // 0x5d
            public ushort OpponentId; // 0x5e
        }
    }

    public sealed class OpponentCSVMap : ClassMap<Opponent.StructureData>
    {
        public OpponentCSVMap()
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
            Map(m => m.Unknown2b);
            Map(m => m.NATuning).TypeConverter(Utils.GetFileNameConverter("NATuning"));
            Map(m => m.TurboKit).TypeConverter(Utils.GetFileNameConverter("TurboKit"));
            Map(m => m.Drivetrain).TypeConverter(Utils.GetFileNameConverter("Drivetrain"));
            Map(m => m.Flywheel).TypeConverter(Utils.GetFileNameConverter("Flywheel"));
            Map(m => m.Clutch).TypeConverter(Utils.GetFileNameConverter("Clutch"));
            Map(m => m.Unknown3);
            Map(m => m.Differential).TypeConverter(Utils.GetFileNameConverter("Differential"));
            Map(m => m.Transmission).TypeConverter(Utils.GetFileNameConverter("Transmission"));
            Map(m => m.Suspension).TypeConverter(Utils.GetFileNameConverter("Suspension"));
            Map(m => m.Unknown4);
            Map(m => m.TyresFront).TypeConverter(Utils.GetFileNameConverter("TyresFront"));
            Map(m => m.TyresRear).TypeConverter(Utils.GetFileNameConverter("TyresRear"));
            Map(m => m.Unknown5);
            Map(m => m.RimsCode3);
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
            Map(m => m.PowerPercentage);
            Map(m => m.Unknown21);
            Map(m => m.OpponentId);
        }
    }
}
