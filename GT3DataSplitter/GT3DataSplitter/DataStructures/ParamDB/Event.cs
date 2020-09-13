using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Event : CsvDataStructure<EventData, EventCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x1F8
    public struct EventData
    {
        public ulong EventID;
        public ulong CourseID;
        public ulong Opponent1;
        public ulong Opponent2;
        public ulong Opponent3;
        public ulong Opponent4;
        public ulong Opponent5;
        public ulong Opponent6;
        public ulong Opponent7;
        public ulong Opponent8;
        public ulong Opponent9;
        public ulong Opponent10;
        public ulong Opponent11;
        public ulong Opponent12;
        public ulong Opponent13;
        public ulong Opponent14;
        public ulong Opponent15;
        public ulong Opponent16;
        public ulong Opponent17;
        public ulong Opponent18;
        public ulong Opponent19;
        public ulong Opponent20;
        public ulong Opponent21;
        public ulong Opponent22;
        public ulong Opponent23;
        public ulong Opponent24;
        public ulong Opponent25;
        public ulong Opponent26;
        public ulong Opponent27;
        public ulong Opponent28;
        public ulong Opponent29;
        public ulong Opponent30;
        public ulong Opponent31;
        public ulong Opponent32;
        public uint UnknownOpponentData1;
        public uint UnknownOpponentData2;
        public uint UnknownOpponentData3;
        public uint UnknownOpponentData4;
        public uint UnknownOpponentData5;
        public uint UnknownOpponentData6;
        public uint UnknownOpponentData7;
        public uint UnknownOpponentData8;
        public uint UnknownOpponentData9;
        public uint UnknownOpponentData10;
        public uint UnknownOpponentData11;
        public uint UnknownOpponentData12;
        public uint UnknownOpponentData13;
        public uint UnknownOpponentData14;
        public uint UnknownOpponentData15;
        public uint UnknownOpponentData16;
        public uint UnknownOpponentData17;
        public uint UnknownOpponentData18;
        public uint UnknownOpponentData19;
        public uint UnknownOpponentData20;
        public uint UnknownOpponentData21;
        public uint UnknownOpponentData22;
        public uint UnknownOpponentData23;
        public uint UnknownOpponentData24;
        public uint UnknownOpponentData25;
        public uint UnknownOpponentData26;
        public uint UnknownOpponentData27;
        public uint UnknownOpponentData28;
        public uint UnknownOpponentData29;
        public uint UnknownOpponentData30;
        public uint UnknownOpponentData31;
        public uint UnknownOpponentData32;
        public ulong EligibleCarsListID;
        public byte RollingStartSpeed;
        public byte Laps;
        public byte SpecialConditionsType; // 1 license accel / stop, 2 license point to point, 3 license laps, 6 Dirt rallies
        public byte License; // 1 B, 2 A, 3 IB, 4 IA, 5 S, 6 R
        public byte AIMaybe1;
        public byte AIMaybe2;
        public byte AIMaybe3;
        public byte AIMaybe4;
        public byte AIMaybe5;
        public byte AIMaybe6;
        public byte AIMaybe7;
        public byte AIMaybe8;
        public byte AIMaybe9;
        public byte AIMaybe10;
        public byte AIMaybe11;
        public byte AIMaybe12;
        public byte AIMaybe13;
        public byte AIMaybe14;
        public byte AIMaybe15;
        public byte AIMaybe16;
        public byte AIMaybe17;
        public byte AIMaybe18;
        public byte AIMaybe19;
        public byte AIMaybe20;
        public byte Unknown1; // always 0
        public byte Unknown2; // always 0
        public byte Unknown3; // always 0
        public byte Unknown4; // always 0
        public byte Unknown5; // always 100
        public byte CarsOnTrack;
        public byte Unknown6; // always 0
        public byte TireWearUnknownMaybe1;
        public byte TireWearUnknownMaybe2;
        public byte TireWearUnknownMaybe3;
        public byte TireWearUnknownMaybe4;
        public byte TireWearUnknownMaybe5;
        public byte TireWearUnknownMaybe6;
        public byte TireWearUnknownMaybe7;
        public byte TireWearUnknownMaybe8;
        public byte TireWearUnknownMaybe9;
        public byte TireWearUnknownMaybe10;
        public byte TireWearUnknownMaybe11;
        public byte TireWearUnknownMaybe12;
        public byte TireWearUnknownMaybe13;
        public byte GoldTimeMinutes;
        public byte GoldTimeSeconds;
        public ushort GoldTimeMilliseconds;
        public byte SilverTimeMinutes;
        public byte SilverTimeSeconds;
        public ushort SilverTimeMilliseconds;
        public byte BronzeTimeMinutes;
        public byte BronzeTimeSeconds;
        public ushort BronzeTimeMilliseconds;
        public ushort Unknown7; // always 0
        public byte TireRestriction;
        public byte Category; // 0 Unknown, 1 Arcade Easy, 2 Arcade Normal, 3 Arcade Hard, 4 Arcade Professional, 5 Beginner, 6 Amateur, 7 Professional / Endurance, 8 Dirt
        public ushort PrizeMoney1st;
        public ushort PrizeMoney2nd;
        public ushort PrizeMoney3rd;
        public ushort PrizeMoney4th;
        public ushort PrizeMoney5th;
        public ushort PrizeMoney6th;
        public ushort TireWearUnknownMaybe14;
        public ushort SeriesChampBonus;
        public byte TimeLimitMinutes;
        public byte DifficultyLevel; // 1 Beginner, 2 Normal, 3 Professional hall
        public byte Unknown8;
        public byte Unknown9; // flags of some sort on time trials and some licenses?
        public ulong RaceModeID;
        public ushort Label;
        public byte MachineTestMaybe; // 0 normally, 7 for machine test
        public byte EventTypeRelatedMaybe; // 0 demos / machine test, 1 early licenses, 2 normal, 3 time trial / endurance / late licenses
        public byte CarTypeRestriction; // 1 racecar, 3 unmodified or roadcar
        public byte AspirationRestriction; // 1 NA, 2 Turbo
        public ushort DrivetrainRestriction; // 1 FF, 2 FR, 3 4WD, 4 MR
    }

    public sealed class EventCSVMap : ClassMap<EventData>
    {
        public EventCSVMap()
        {
            Map(m => m.EventID).TypeConverter(Utils.IdConverter);
            Map(m => m.CourseID).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent1).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent2).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent3).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent4).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent5).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent6).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent7).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent8).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent9).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent10).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent11).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent12).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent13).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent14).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent15).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent16).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent17).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent18).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent19).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent20).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent21).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent22).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent23).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent24).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent25).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent26).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent27).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent28).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent29).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent30).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent31).TypeConverter(Utils.IdConverter);
            Map(m => m.Opponent32).TypeConverter(Utils.IdConverter);
            Map(m => m.UnknownOpponentData1);
            Map(m => m.UnknownOpponentData2);
            Map(m => m.UnknownOpponentData3);
            Map(m => m.UnknownOpponentData4);
            Map(m => m.UnknownOpponentData5);
            Map(m => m.UnknownOpponentData6);
            Map(m => m.UnknownOpponentData7);
            Map(m => m.UnknownOpponentData8);
            Map(m => m.UnknownOpponentData9);
            Map(m => m.UnknownOpponentData10);
            Map(m => m.UnknownOpponentData11);
            Map(m => m.UnknownOpponentData12);
            Map(m => m.UnknownOpponentData13);
            Map(m => m.UnknownOpponentData14);
            Map(m => m.UnknownOpponentData15);
            Map(m => m.UnknownOpponentData16);
            Map(m => m.UnknownOpponentData17);
            Map(m => m.UnknownOpponentData18);
            Map(m => m.UnknownOpponentData19);
            Map(m => m.UnknownOpponentData20);
            Map(m => m.UnknownOpponentData21);
            Map(m => m.UnknownOpponentData22);
            Map(m => m.UnknownOpponentData23);
            Map(m => m.UnknownOpponentData24);
            Map(m => m.UnknownOpponentData25);
            Map(m => m.UnknownOpponentData26);
            Map(m => m.UnknownOpponentData27);
            Map(m => m.UnknownOpponentData28);
            Map(m => m.UnknownOpponentData29);
            Map(m => m.UnknownOpponentData30);
            Map(m => m.UnknownOpponentData31);
            Map(m => m.UnknownOpponentData32);
            Map(m => m.EligibleCarsListID).TypeConverter(Utils.IdConverter);
            Map(m => m.RollingStartSpeed);
            Map(m => m.Laps);
            Map(m => m.SpecialConditionsType);
            Map(m => m.License);
            Map(m => m.AIMaybe1);
            Map(m => m.AIMaybe2);
            Map(m => m.AIMaybe3);
            Map(m => m.AIMaybe4);
            Map(m => m.AIMaybe5);
            Map(m => m.AIMaybe6);
            Map(m => m.AIMaybe7);
            Map(m => m.AIMaybe8);
            Map(m => m.AIMaybe9);
            Map(m => m.AIMaybe10);
            Map(m => m.AIMaybe11);
            Map(m => m.AIMaybe12);
            Map(m => m.AIMaybe13);
            Map(m => m.AIMaybe14);
            Map(m => m.AIMaybe15);
            Map(m => m.AIMaybe16);
            Map(m => m.AIMaybe17);
            Map(m => m.AIMaybe18);
            Map(m => m.AIMaybe19);
            Map(m => m.AIMaybe20);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Unknown5);
            Map(m => m.CarsOnTrack);
            Map(m => m.Unknown6);
            Map(m => m.TireWearUnknownMaybe1);
            Map(m => m.TireWearUnknownMaybe2);
            Map(m => m.TireWearUnknownMaybe3);
            Map(m => m.TireWearUnknownMaybe4);
            Map(m => m.TireWearUnknownMaybe5);
            Map(m => m.TireWearUnknownMaybe6);
            Map(m => m.TireWearUnknownMaybe7);
            Map(m => m.TireWearUnknownMaybe8);
            Map(m => m.TireWearUnknownMaybe9);
            Map(m => m.TireWearUnknownMaybe10);
            Map(m => m.TireWearUnknownMaybe11);
            Map(m => m.TireWearUnknownMaybe12);
            Map(m => m.TireWearUnknownMaybe13);
            Map(m => m.GoldTimeMinutes);
            Map(m => m.GoldTimeSeconds);
            Map(m => m.GoldTimeMilliseconds);
            Map(m => m.SilverTimeMinutes);
            Map(m => m.SilverTimeSeconds);
            Map(m => m.SilverTimeMilliseconds);
            Map(m => m.BronzeTimeMinutes);
            Map(m => m.BronzeTimeSeconds);
            Map(m => m.BronzeTimeMilliseconds);
            Map(m => m.Unknown7);
            Map(m => m.TireRestriction);
            Map(m => m.Category);
            Map(m => m.PrizeMoney1st);
            Map(m => m.PrizeMoney2nd);
            Map(m => m.PrizeMoney3rd);
            Map(m => m.PrizeMoney4th);
            Map(m => m.PrizeMoney5th);
            Map(m => m.PrizeMoney6th);
            Map(m => m.TireWearUnknownMaybe14);
            Map(m => m.SeriesChampBonus);
            Map(m => m.TimeLimitMinutes);
            Map(m => m.DifficultyLevel);
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.RaceModeID).TypeConverter(Utils.IdConverter);
            Map(m => m.Label).TypeConverter(Program.Strings.Lookup);
            Map(m => m.MachineTestMaybe);
            Map(m => m.EventTypeRelatedMaybe);
            Map(m => m.CarTypeRestriction);
            Map(m => m.AspirationRestriction);
            Map(m => m.DrivetrainRestriction);
        }
    }
}