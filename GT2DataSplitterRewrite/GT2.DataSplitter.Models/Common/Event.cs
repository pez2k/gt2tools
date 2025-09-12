using GT2.DataSplitter.Models.Enums;

namespace GT2.DataSplitter.Models.Common
{
    public class Event
    {
        public string EventName { get; set; } = "";
        public string TrackName { get; set; } = "";
        public uint Opponent1 { get; set; }
        public uint Opponent2 { get; set; }
        public uint Opponent3 { get; set; }
        public uint Opponent4 { get; set; }
        public uint Opponent5 { get; set; }
        public uint Opponent6 { get; set; }
        public uint Opponent7 { get; set; }
        public uint Opponent8 { get; set; }
        public uint Opponent9 { get; set; }
        public uint Opponent10 { get; set; }
        public uint Opponent11 { get; set; }
        public uint Opponent12 { get; set; }
        public uint Opponent13 { get; set; }
        public uint Opponent14 { get; set; }
        public uint Opponent15 { get; set; }
        public uint Opponent16 { get; set; }
        public byte RollingStartSpeed { get; set; }
        public byte Laps { get; set; }
        public byte AutoDrive { get; set; }
        public Licence Licence { get; set; }
        public byte AIFrontGripRWD { get; set; }
        public byte AIFrontGripFWD { get; set; }
        public byte AIFrontGrip4WD { get; set; }
        public byte AIFrontGripSpecial4WD { get; set; }
        public byte AIRearGripRWD { get; set; }
        public byte AIRearGripFWD { get; set; }
        public byte AIRearGrip4WD { get; set; }
        public byte AIRearGripSpecial4WD { get; set; }
        public byte AIAccelerationRWD { get; set; }
        public byte AIAccelerationFWD { get; set; }
        public byte AIAcceleration4WD { get; set; }
        public byte AIAccelerationSpecial4WD { get; set; }
        public byte AIThrottleLiftReductionRWD { get; set; }
        public byte AIThrottleLiftReductionFWD { get; set; }
        public byte AIThrottleLiftReduction4WD { get; set; }
        public byte AIThrottleLiftReductionSpecial4WD { get; set; }
        public byte AIRubberBandMultiplier { get; set; }
        public byte AIRubberBandUnknown1 { get; set; }
        public byte AIRubberBandScaledPerCar { get; set; }
        public byte AIRubberBandLeadingSlowdownPercentage { get; set; }
        public ushort AIRubberBandLeadingScalingDistance { get; set; }
        public byte AIRubberBandTrailingSpeedupPercentage { get; set; }
        public ushort AIRubberBandTrailingScalingDistance { get; set; }
        public byte TireWearOrangeDurationMultiplier { get; set; }
        public byte TireWearOrangeGripLoss { get; set; }
        public byte TireWearUnknown { get; set; }
        public byte TireWearBlueDurationMultiplier { get; set; }
        public byte TireWearBlueGripLoss { get; set; }
        public byte TireWearGreenDurationMultiplier { get; set; }
        public byte TireWearGreenGripLoss { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
        public byte Unknown4 { get; set; }
        public bool IsRally { get; set; }
        public byte EligibleCarsRestriction { get; set; }
        public DrivetrainType DrivetrainRestriction { get; set; }
        public ushort PrizeMoney1st { get; set; }
        public ushort PrizeMoney2nd { get; set; }
        public ushort PrizeMoney3rd { get; set; }
        public ushort PrizeMoney4th { get; set; }
        public ushort PrizeMoney5th { get; set; }
        public ushort PrizeMoney6th { get; set; }
        public uint[] PrizeCars { get; set; } = [];
        public string TrackBannerPool { get; set; } = "";
        public ushort PSRestriction { get; set; }
        public ushort SeriesChampBonus { get; set; }
        public ushort CarRestrictionFlags { get; set; }
    }
}