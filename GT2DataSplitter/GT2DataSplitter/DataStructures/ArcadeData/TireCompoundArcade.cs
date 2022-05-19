namespace GT2.DataSplitter
{
    public class TireCompoundArcade : TireCompound
    {
        public TireCompoundArcade() : base()
        {
            filenameCacheNameOverride = nameof(TireCompound);
            tireCompoundNames = new string[] { "SportsFront", "SportsRear", "LowGripFront", "LowGripRear", "DirtFront", "DirtRear",
                                               "DriftFront", "DriftRear", "RWDDirtFront", "RWDDirtRear" };
        }
    }
}