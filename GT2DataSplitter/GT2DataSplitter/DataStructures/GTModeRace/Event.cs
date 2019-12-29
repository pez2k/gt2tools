using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class Event : CsvDataStructure<EventData, EventCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            string directory = Name;
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory + "\\" + RaceStringTable.Lookup.ConvertToString(Data.EventName, null, null) +
                   "_" + RaceStringTable.Lookup.ConvertToString(Data.TrackName, null, null) + ".csv";
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x9C
    public struct EventData
    {
        public ushort EventName; // 0
        public ushort TrackName; // 2
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
        public byte AutoDrive; // (0x46) - 1 = no control of car (but camera change, pausing still work)
        public byte Licence; // 1 = B, 2 = A, 3 = IC, 4 = IB, 5 = IA, 6 = S (0x47)
        public byte AIFrontGripRWD; // 17 0x64's (0x48), changing to all 0 = nothing
        public byte AIFrontGripFWD;
        public byte AIFrontGrip4WD;
        public byte AIFrontGripSpecial4WD;
        public byte AIRearGripRWD;
        public byte AIRearGripFWD;
        public byte AIRearGrip4WD;
        public byte AIRearGripSpecial4WD;
        public byte AIAccelerationRWD;
        public byte AIAccelerationFWD;
        public byte AIAcceleration4WD;
        public byte AIAccelerationSpecial4WD;
        public byte AIThrottleLiftReductionRWD;
        public byte AIThrottleLiftReductionFWD;
        public byte AIThrottleLiftReduction4WD;
        public byte AIThrottleLiftReductionSpecial4WD;
        public byte AIRubberBandMultiplier;
        public byte AIRubberBandUnknown1; // (0x59) - changing to 0x100 didn't seem to do anything, likewise 0x1
        public byte AIRubberBandScaledPerCar;
        public byte AIRubberBandLeadingSlowdownPercentage; // (0x5b) - changing  it to 0 or 0xff doesn't seem to do anything
        public ushort AIRubberBandLeadingScalingDistance; // (0x5c)
        public byte AIRubberBandTrailingSpeedupPercentage; // (0x5e) 
        public ushort AIRubberBandTrailingScalingDistance; // (0x5f)
        public byte TireWearOrangeDurationMultiplier; // (0x60)
        public byte TireWearOrangeGripLoss; // 0x6400 above (0x62) - changed unk4 to unk10 to FF, spaced out opponents (power related?), changed all to 0, same as ff really
        public byte TireWearUnknown;
        public byte TireWearBlueDurationMultiplier;
        public byte TireWearBlueGripLoss;
        public byte TireWearGreenDurationMultiplier;
        public byte TireWearGreenGripLoss;
        public uint Unknown1; // (0x68)
        public uint Unknown2; // (0x6c)
        public uint Unknown3; // (0x70)
        public byte Unknown4; // (0x74)
        public byte IsRally; // 0x75 set to 1 for rally race - requires dirt tyres, only 1 opponent. Can award prize car
        public byte EligibleCarsRestriction; // (0x75) - index into allowable entrants list
        public byte DrivetrainRestriction; // (0x76) 1 = FF, 2 = FR, 3 = MR, 4 = RR, 5 = 4WD
        public ushort PrizeMoney1st; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x78)
        public ushort PrizeMoney2nd; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x7a)
        public ushort PrizeMoney3rd; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x7c)
        public ushort PrizeMoney4th; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x7e)
        public ushort PrizeMoney5th; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x80)
        public ushort PrizeMoney6th; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x82)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] PrizeCars; // ids of cars (0x84)
        public ushort Unknown5; // (0x94)
        public ushort PSRestriction; // in ps units (hp = ps / 1.01427772651) (0x96)
        public ushort SeriesChampBonus; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x98)
        public ushort CarRestrictionFlags; // (0x9a) flags to restrict the type of car you use for this race. 0x100 = non-race car, 0x200 = just race car
    }

    public sealed class EventCSVMap : ClassMap<EventData>
    {
        public EventCSVMap()
        {
            Map(m => m.EventName).TypeConverter(RaceStringTable.Lookup);
            Map(m => m.TrackName).TypeConverter(RaceStringTable.Lookup);
            Map(m => m.Opponent1).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent2).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent3).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent4).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent5).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent6).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent7).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent8).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent9).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent10).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent11).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent12).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent13).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent14).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent15).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.Opponent16).TypeConverter(Utils.GetFileNameConverter(nameof(EnemyCars)));
            Map(m => m.RollingStartSpeed);
            Map(m => m.Laps);
            Map(m => m.AutoDrive);
            Map(m => m.Licence).TypeConverter(Utils.LicenseConverter);
            Map(m => m.AIFrontGripRWD);
            Map(m => m.AIFrontGripFWD);
            Map(m => m.AIFrontGrip4WD);
            Map(m => m.AIFrontGripSpecial4WD);
            Map(m => m.AIRearGripRWD);
            Map(m => m.AIRearGripFWD);
            Map(m => m.AIRearGrip4WD);
            Map(m => m.AIRearGripSpecial4WD);
            Map(m => m.AIAccelerationRWD);
            Map(m => m.AIAccelerationFWD);
            Map(m => m.AIAcceleration4WD);
            Map(m => m.AIAccelerationSpecial4WD);
            Map(m => m.AIThrottleLiftReductionRWD);
            Map(m => m.AIThrottleLiftReductionFWD);
            Map(m => m.AIThrottleLiftReduction4WD);
            Map(m => m.AIThrottleLiftReductionSpecial4WD);
            Map(m => m.AIRubberBandMultiplier);
            Map(m => m.AIRubberBandUnknown1);
            Map(m => m.AIRubberBandScaledPerCar);
            Map(m => m.AIRubberBandLeadingSlowdownPercentage);
            Map(m => m.AIRubberBandLeadingScalingDistance);
            Map(m => m.AIRubberBandTrailingSpeedupPercentage);
            Map(m => m.AIRubberBandTrailingScalingDistance);
            Map(m => m.TireWearOrangeDurationMultiplier);
            Map(m => m.TireWearOrangeGripLoss);
            Map(m => m.TireWearUnknown);
            Map(m => m.TireWearBlueDurationMultiplier);
            Map(m => m.TireWearBlueGripLoss);
            Map(m => m.TireWearGreenDurationMultiplier);
            Map(m => m.TireWearGreenGripLoss);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.IsRally);
            Map(m => m.EligibleCarsRestriction).TypeConverter(Utils.GetFileNameConverter(nameof(Regulations)));
            Map(m => m.DrivetrainRestriction).TypeConverter(Utils.DrivetrainRestrictionConverter);
            Map(m => m.PrizeMoney1st);
            Map(m => m.PrizeMoney2nd);
            Map(m => m.PrizeMoney3rd);
            Map(m => m.PrizeMoney4th);
            Map(m => m.PrizeMoney5th);
            Map(m => m.PrizeMoney6th);
            Map(m => m.PrizeCars).TypeConverter(Utils.CarIdArrayConverter);
            Map(m => m.Unknown5).TypeConverter(RaceStringTable.Lookup);
            Map(m => m.PSRestriction);
            Map(m => m.SeriesChampBonus);
            Map(m => m.CarRestrictionFlags); // 1 for NA, 2 for Turbo, 256 for Normal, 512 for Race
        }
    }
}
