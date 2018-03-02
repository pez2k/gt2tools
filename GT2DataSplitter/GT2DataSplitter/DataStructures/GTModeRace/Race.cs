using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2DataSplitter
{
    public class Race : CsvDataStructure
    {
        public Race()
        {
            Size = 0x9C;
        }
        
        public override void Read(FileStream infile)
        {
            Data = ReadStructure<StructureData>(infile);
        }

        public override void Dump()
        {
            DumpCsv<StructureData, RaceCSVMap>(Data);
        }

        public override void Import(string filename)
        {
            Data = ImportCsv<StructureData, RaceCSVMap>(filename);
        }

        public override void Write(FileStream outfile)
        {
            WriteStructure(outfile, Data);
        }

        public StructureData Data { get; set; }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public new class StructureData : CsvDataStructure.StructureData
        {
            public ushort RaceNameIndex; // 0
            public ushort TrackNameId; // 2
            public uint Opponent1; // 4
            public uint Opponent2; // 8
            public uint Opponent3; // 0xc
            public uint Opponent4; // 0x10
            public uint Opponent5; // 0x14
            public uint Opponent6; // 0x18
            public uint Opponent7; // 0x1c
            public uint Opponent8; // 0x20
            public uint Opponent9; // 0x24
            public uint Opponent10; // 0x28
            public uint Opponent11; // 0x2c
            public uint Opponent12; // 0x30
            public uint Opponent13; // 0x34
            public uint Opponent14; // 0x38
            public uint Opponent15; // 0x3c
            public uint Opponent16; // 0x40
            public byte RollingStartSpeed; // (0x44) - 0 = normal standing start
            public byte Laps; // (0x45)
            public byte Unknown1; // (0x46) - 1 = no control of car (but camera change, pausing still work)
            public byte Licence; // 1 = B, 2 = A, 3 = IC, 4 = IB, 5 = IA, 6 = S (0x47)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public byte[] Unknown2; // 17 0x64's (0x48), changing to all 0 = nothing
            public ushort Unknown3; // (0x59) - changing to 0x100 didn't seem to do anything, likewise 0x1
            public byte Unknown4; // (0x5b) - changing  it to 0 or 0xff doesn't seem to do anything
            public ushort Unknown5; // (0x5c)
            public byte Unknown6; // (0x5e) 
            public byte Unknown7; // (0x5f)
            public ushort Unknown8; // (0x60)
            public ushort Unknown9; // 0x6400 above (0x62) - changed unk4 to unk10 to FF, spaced out opponents (power related?), changed all to 0, same as ff really
            public uint Unknown10; // (0x64)
            public uint Unknown11; // (0x68)
            public uint Unknown12; // (0x6c)
            public uint Unknown13; // (0x70)
            public byte Unknown14; // (0x74)
            public byte IsRally; // 0x75 set to 1 for rally race - requires dirt tyres, only 1 opponent. Can award prize car
            public byte AllowedEntrantsId; // (0x75) - index into allowable entrants list
            public byte ForcedDriveTrainFlags; // (0x76) 1 = FF, 2 = FR, 3 = MR, 4 = RR, 5 = 4WD
            public ushort PrizeMoney1st; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x78)
            public ushort PrizeMoney2nd; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x7a)
            public ushort PrizeMoney3rd; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x7c)
            public ushort PrizeMoney4th; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x7e)
            public ushort PrizeMoney5th; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x80)
            public ushort PrizeMoney6th; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x82)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] PrizeCars; // ids of cars (0x84)
            public ushort Unknown15; // (0x94)
            public ushort PSRestriction; // in ps units (hp = ps / 1.01427772651) (0x96)
            public ushort SeriesChampBonus; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x98)
            public ushort CarRestrictionFlags; // (0x9a) flags to restrict the type of car you use for this race. 0x100 = non-race car, 0x200 = just race car
        }
    }

    public sealed class RaceCSVMap : ClassMap<Race.StructureData>
    {
        public RaceCSVMap()
        {
            Map(m => m.RaceNameIndex);
            Map(m => m.TrackNameId);
            Map(m => m.Opponent1);
            Map(m => m.Opponent2);
            Map(m => m.Opponent3);
            Map(m => m.Opponent4);
            Map(m => m.Opponent5);
            Map(m => m.Opponent6);
            Map(m => m.Opponent7);
            Map(m => m.Opponent8);
            Map(m => m.Opponent9);
            Map(m => m.Opponent10);
            Map(m => m.Opponent11);
            Map(m => m.Opponent12);
            Map(m => m.Opponent13);
            Map(m => m.Opponent14);
            Map(m => m.Opponent15);
            Map(m => m.Opponent16);
            Map(m => m.RollingStartSpeed);
            Map(m => m.Laps);
            Map(m => m.Unknown1);
            Map(m => m.Licence);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.Unknown10);
            Map(m => m.Unknown11);
            Map(m => m.Unknown12);
            Map(m => m.Unknown13);
            Map(m => m.Unknown14);
            Map(m => m.IsRally);
            Map(m => m.AllowedEntrantsId);
            Map(m => m.ForcedDriveTrainFlags);
            Map(m => m.PrizeMoney1st);
            Map(m => m.PrizeMoney2nd);
            Map(m => m.PrizeMoney3rd);
            Map(m => m.PrizeMoney4th);
            Map(m => m.PrizeMoney5th);
            Map(m => m.PrizeMoney6th);
            Map(m => m.PrizeCars).TypeConverter(Utils.CarIdArrayConverter);
            Map(m => m.Unknown15);
            Map(m => m.PSRestriction);
            Map(m => m.SeriesChampBonus);
            Map(m => m.CarRestrictionFlags);
        }
    }
}
