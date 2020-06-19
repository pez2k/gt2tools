using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class TireSize : CarCsvDataStructure<TireSizeData, TireSizeCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x04
    public struct TireSizeData
    {
        public byte DiameterInches;
        public byte WidthMM;
        public byte Profile;
    }

    public sealed class TireSizeCSVMap : ClassMap<TireSizeData>
    {
        public TireSizeCSVMap()
        {
            Map(m => m.DiameterInches);
            Map(m => m.WidthMM);
            Map(m => m.Profile);
        }
    }
}
