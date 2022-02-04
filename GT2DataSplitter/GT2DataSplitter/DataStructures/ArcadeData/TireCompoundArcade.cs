using System.Collections.Generic;

namespace GT2.DataSplitter
{
    public class TireCompoundArcade : TireCompound
    {
        public TireCompoundArcade() : base()
        {
            filenameCacheNameOverride = nameof(TireCompound);
            tireCompoundNames = new List<string> { "SportsFront", "SportsRear", "LowGripFront", "LowGripRear", "DirtFront", "DirtRear",
                                                   "HighGripFront", "HighGripRear", "RWDDirtFront", "RWDDirtRear" };
        }
    }
}