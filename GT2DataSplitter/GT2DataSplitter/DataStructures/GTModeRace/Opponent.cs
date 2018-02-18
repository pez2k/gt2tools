﻿using CsvHelper;
using CsvHelper.Configuration;
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
            public uint NATuning; // 1a
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
            Map(m => m.Brakes);
            Map(m => m.IsSpecial);
            Map(m => m.WeightReduction);
            Map(m => m.Unknown0);
            Map(m => m.Body);
            Map(m => m.Engine);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.NATuning);
            Map(m => m.TurboKit);
            Map(m => m.Drivetrain);
            Map(m => m.Flywheel);
            Map(m => m.Clutch);
            Map(m => m.Unknown3);
            Map(m => m.Differential);
            Map(m => m.Transmission);
            Map(m => m.Suspension);
            Map(m => m.Unknown4);
            Map(m => m.TyresFront);
            Map(m => m.TyresRear);
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
