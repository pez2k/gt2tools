using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class EligibleCars : CsvDataStructure<EligibleCarsData, EligibleCarsCSVMap>
    {
        public EligibleCars()
        {
            CacheFilename = true;
        }

        public override void Dump()
        {
            if (!FileNameCache.Cache.ContainsKey(Name))
            {
                FileNameCache.Add(Name, "None");
            }
            base.Dump();
        }

        public override void Import(string filename)
        {
            if (!FileNameCache.Cache.ContainsKey(Name))
            {
                FileNameCache.Add(Name, "None");
            }
            base.Import(filename);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x80
    public struct EligibleCarsData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public uint[] EligibleCarIds;
    }

    public sealed class EligibleCarsCSVMap : ClassMap<EligibleCarsData>
    {
        public EligibleCarsCSVMap()
        {
            Map(m => m.EligibleCarIds).TypeConverter(Utils.CarIdArrayConverter);
        }
    }
}
