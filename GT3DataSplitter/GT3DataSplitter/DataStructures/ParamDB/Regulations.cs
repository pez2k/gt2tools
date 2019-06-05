using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Regulations : CsvDataStructure<RegulationsData, RegulationsCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x108
    public struct RegulationsData
    {
        public ulong Regulations;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public ulong[] EligibleCars;
    }

    public sealed class RegulationsCSVMap : ClassMap<RegulationsData>
    {
        public RegulationsCSVMap()
        {
            Map(m => m.Regulations).TypeConverter(Utils.IdConverter);
            Map(m => m.EligibleCars).TypeConverter(Utils.IdArrayConverter);
        }
    }
}