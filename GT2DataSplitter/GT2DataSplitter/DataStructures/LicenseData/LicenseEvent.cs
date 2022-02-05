using System.IO;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using TypeConverters;

    public class LicenseEvent : CsvDataStructure<EventData, LicenseEventCSVMap>
    {
        protected override string CreateOutputFilename()
        {
            string directory = Name;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory + "\\" + new RaceStringTableLookup().ConvertToString(data.EventName, null, null) +
                   "_" + new RaceStringTableLookup().ConvertToString(data.TrackName, null, null) + ".csv";
        }
    }

    public sealed class LicenseEventCSVMap : ClassMap<EventData>
    {
        public LicenseEventCSVMap()
        {
            Map(m => m.EventName).TypeConverter(new RaceStringTableLookup());
            Map(m => m.TrackName).TypeConverter(new RaceStringTableLookup());
            Map(m => m.Opponent1); // TODO: clearly not opponent IDs as there are no opponents - fix this
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
            Map(m => m.AutoDrive);
            Map(m => m.Licence).TypeConverter(new LicenseConverter());
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
            Map(m => m.EligibleCarsRestriction).PartFilename(nameof(Regulations));
            Map(m => m.DrivetrainRestriction).TypeConverter(new DrivetrainRestrictionConverter());
            Map(m => m.PrizeMoney1st);
            Map(m => m.PrizeMoney2nd);
            Map(m => m.PrizeMoney3rd);
            Map(m => m.PrizeMoney4th);
            Map(m => m.PrizeMoney5th);
            Map(m => m.PrizeMoney6th);
            Map(m => m.PrizeCars).TypeConverter(new CarIdArrayConverter());
            Map(m => m.TrackBannerPool).TypeConverter(new RaceStringTableLookup()); // references TIM list in .crstims.tsd, or "0" for static unrandomised banners
            Map(m => m.PSRestriction);
            Map(m => m.SeriesChampBonus);
            Map(m => m.CarRestrictionFlags); // 1 for NA, 2 for Turbo, 256 for Normal, 512 for Race
        }
    }
}