using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class TireSize : CsvDataStructure<TireSizeData, TireSizeCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
    public struct TireSizeData
    {
        public ulong Part;
        public byte DiameterInches;
        public byte Profile;
        public byte WidthMM;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] Padding;
    }

    public sealed class TireSizeCSVMap : ClassMap<TireSizeData>
    {
        public TireSizeCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.DiameterInches);
            Map(m => m.Profile); // * 5
            Map(m => m.WidthMM); // * 5
        }
    }
}