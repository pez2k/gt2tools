using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using Models.Enums;

    public class Event : MappedDataStructure<Event.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x9C
        public struct Data
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
            public ushort TrackBannerPool; // (0x94)
            public ushort PSRestriction; // in ps units (hp = ps / 1.01427772651) (0x96)
            public ushort SeriesChampBonus; // multiply by 100 for non-JP / multiply by 10,000 for JP (0x98)
            public ushort CarRestrictionFlags; // (0x9a) flags to restrict the type of car you use for this race. 0x100 = non-race car, 0x200 = just race car
        }

        public Models.Common.Event MapToModel(ASCIIStringTable strings) =>
            new Models.Common.Event
            {
                EventName = strings.Get(data.EventName),
                TrackName = strings.Get(data.TrackName),
                Opponent1 = data.Opponent1,
                Opponent2 = data.Opponent2,
                Opponent3 = data.Opponent3,
                Opponent4 = data.Opponent4,
                Opponent5 = data.Opponent5,
                Opponent6 = data.Opponent6,
                Opponent7 = data.Opponent7,
                Opponent8 = data.Opponent8,
                Opponent9 = data.Opponent9,
                Opponent10 = data.Opponent10,
                Opponent11 = data.Opponent11,
                Opponent12 = data.Opponent12,
                Opponent13 = data.Opponent13,
                Opponent14 = data.Opponent14,
                Opponent15 = data.Opponent15,
                Opponent16 = data.Opponent16,
                RollingStartSpeed = data.RollingStartSpeed,
                Laps = data.Laps,
                AutoDrive = data.AutoDrive,
                Licence = (Licence)data.Licence,
                AIFrontGripRWD = data.AIFrontGripRWD,
                AIFrontGripFWD = data.AIFrontGripFWD,
                AIFrontGrip4WD = data.AIFrontGrip4WD,
                AIFrontGripSpecial4WD = data.AIFrontGripSpecial4WD,
                AIRearGripRWD = data.AIRearGripRWD,
                AIRearGripFWD = data.AIRearGripFWD,
                AIRearGrip4WD = data.AIRearGrip4WD,
                AIRearGripSpecial4WD = data.AIRearGripSpecial4WD,
                AIAccelerationRWD = data.AIAccelerationRWD,
                AIAccelerationFWD = data.AIAccelerationFWD,
                AIAcceleration4WD = data.AIAcceleration4WD,
                AIAccelerationSpecial4WD = data.AIAccelerationSpecial4WD,
                AIThrottleLiftReductionRWD = data.AIThrottleLiftReductionRWD,
                AIThrottleLiftReductionFWD = data.AIThrottleLiftReductionFWD,
                AIThrottleLiftReduction4WD = data.AIThrottleLiftReduction4WD,
                AIThrottleLiftReductionSpecial4WD = data.AIThrottleLiftReductionSpecial4WD,
                AIRubberBandMultiplier = data.AIRubberBandMultiplier,
                AIRubberBandUnknown1 = data.AIRubberBandUnknown1,
                AIRubberBandScaledPerCar = data.AIRubberBandScaledPerCar,
                AIRubberBandLeadingSlowdownPercentage = data.AIRubberBandLeadingSlowdownPercentage,
                AIRubberBandLeadingScalingDistance = data.AIRubberBandLeadingScalingDistance,
                AIRubberBandTrailingSpeedupPercentage = data.AIRubberBandTrailingSpeedupPercentage,
                AIRubberBandTrailingScalingDistance = data.AIRubberBandTrailingScalingDistance,
                TireWearOrangeDurationMultiplier = data.TireWearOrangeDurationMultiplier,
                TireWearOrangeGripLoss = data.TireWearOrangeGripLoss,
                TireWearUnknown = data.TireWearUnknown,
                TireWearBlueDurationMultiplier = data.TireWearBlueDurationMultiplier,
                TireWearBlueGripLoss = data.TireWearBlueGripLoss,
                TireWearGreenDurationMultiplier = data.TireWearGreenDurationMultiplier,
                TireWearGreenGripLoss = data.TireWearGreenGripLoss,
                Unknown1 = data.Unknown1,
                Unknown2 = data.Unknown2,
                Unknown3 = data.Unknown3,
                Unknown4 = data.Unknown4,
                IsRally = data.IsRally == 1,
                EligibleCarsRestriction = data.EligibleCarsRestriction,
                DrivetrainRestriction = (DrivetrainType)data.DrivetrainRestriction,
                PrizeMoney1st = data.PrizeMoney1st,
                PrizeMoney2nd = data.PrizeMoney2nd,
                PrizeMoney3rd = data.PrizeMoney3rd,
                PrizeMoney4th = data.PrizeMoney4th,
                PrizeMoney5th = data.PrizeMoney5th,
                PrizeMoney6th = data.PrizeMoney6th,
                PrizeCars = data.PrizeCars,
                TrackBannerPool = strings.Get(data.TrackBannerPool),
                PSRestriction = data.PSRestriction,
                SeriesChampBonus = data.SeriesChampBonus,
                CarRestrictionFlags = data.CarRestrictionFlags
            };
    }
}