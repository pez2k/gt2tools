using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2DataSplitter
{
    public class Opponent : CsvDataStructure
    {
        public Opponent()
        {
            Size = 0x60;
        }
        
        public override void Read(FileStream infile)
        {
            Data = ReadStructure<StructureData>(infile);
        }

        public override void Dump()
        {
            DumpCsv<StructureData, OpponentCSVMap>(Data);
        }

        public override void Import(string filename)
        {
            Data = ImportCsv<StructureData, OpponentCSVMap>(filename);
        }

        public override void Write(FileStream outfile)
        {
            WriteStructure(outfile, Data);
        }

        public StructureData Data { get; set; }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public new class StructureData : CsvDataStructure.StructureData
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
            Map(m => m.Brakes).PartFilename("Brakes");
            Map(m => m.IsSpecial);
            Map(m => m.WeightReduction).PartFilename("WeightReduction");
            Map(m => m.Unknown0);
            Map(m => m.Body).PartFilename("Body");
            Map(m => m.Engine).PartFilename("Engine");
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown2b);
            Map(m => m.NATuning).PartFilename("NATuning");
            Map(m => m.TurboKit).PartFilename("TurboKit");
            Map(m => m.Drivetrain).PartFilename("Drivetrain");
            Map(m => m.Flywheel).PartFilename("Flywheel");
            Map(m => m.Clutch).PartFilename("Clutch");
            Map(m => m.Unknown3);
            Map(m => m.Differential).PartFilename("Differential");
            Map(m => m.Transmission).PartFilename("Transmission");
            Map(m => m.Suspension).PartFilename("Suspension");
            Map(m => m.Unknown4);
            Map(m => m.TyresFront).PartFilename("TyresFront");
            Map(m => m.TyresRear).PartFilename("TyresRear");
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
