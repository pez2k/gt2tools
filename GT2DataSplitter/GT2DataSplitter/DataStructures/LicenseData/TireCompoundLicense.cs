namespace GT2.DataSplitter
{
    public class TireCompoundLicense : TireCompound
    {
        public TireCompoundLicense() : base()
        {
            filenameCacheNameOverride = nameof(TireCompound);
            tireCompoundNames = new string[] { "RoadFront", "RoadRear", "SportsFront", "SportsRear", "HardFront", "HardRear", "MediumFront", "MediumRear",
                                               "SoftFront", "SoftRear", "SuperSoftFront", "SuperSoftRear", "DirtFront", "DirtRear", "RWDDirtFront", "RWDDirtRear",
                                               "PikesPeakFront", "PikesPeakRear", "LowGripFront", "LowGripRear", "Sports2Front", "Sports2Rear" };
        }
    }
}