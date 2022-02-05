using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using TypeConverters;

    public class TireSize : CsvDataStructure<TireSizeData, TireSizeCSVMap>
    {
        public TireSize() => cacheFilename = true;

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return Path.Combine(Path.GetDirectoryName(filename),
                                $"{Path.GetFileNameWithoutExtension(filename).Substring(1)}_{new TireWidthConverter().ConvertToString(data.WidthMM, null, null)}" +
                                $"-{new TireProfileConverter().ConvertToString(data.Profile, null, null)}R{data.DiameterInches}{Path.GetExtension(filename)}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x04
    public struct TireSizeData
    {
        public byte DiameterInches;
        public byte WidthMM;
        public byte Profile;
        public byte Padding;
    }

    public sealed class TireSizeCSVMap : ClassMap<TireSizeData>
    {
        public TireSizeCSVMap()
        {
            Map(m => m.DiameterInches);
            Map(m => m.WidthMM).TypeConverter(new TireWidthConverter());
            Map(m => m.Profile).TypeConverter(new TireProfileConverter());
        }
    }
}