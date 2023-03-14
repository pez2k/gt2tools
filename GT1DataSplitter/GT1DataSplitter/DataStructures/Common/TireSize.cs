using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class TireSize : CsvDataStructure<TireSizeData, TireSizeCSVMap>
    {
        public TireSize()
        {
            Header = "TIRESIZ";
            cacheFilename = true;
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return Path.Combine(Path.GetDirectoryName(filename),
                                $"{Path.GetFileNameWithoutExtension(filename)[1..]}" +
                                $"_{new TireWidthConverter().ConvertToString(data.FrontWidthMM, null, null)}-{new TireProfileConverter().ConvertToString(data.FrontProfile, null, null)}R{data.FrontDiameterInches}" +
                                $"_{new TireWidthConverter().ConvertToString(data.RearWidthMM, null, null)}-{new TireProfileConverter().ConvertToString(data.RearProfile, null, null)}R{data.RearDiameterInches}" +
                                $"{Path.GetExtension(filename)}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x08
    public struct TireSizeData
    {
        public byte FrontProfile;
        public byte FrontDiameterInches;
        public byte FrontWidthMM;
        public byte RearProfile;
        public byte RearDiameterInches;
        public byte RearWidthMM;
        public ushort Padding;
    }

    public sealed class TireSizeCSVMap : ClassMap<TireSizeData>
    {
        public TireSizeCSVMap()
        {
            Map(m => m.FrontProfile).TypeConverter(new TireProfileConverter());
            Map(m => m.FrontDiameterInches);
            Map(m => m.FrontWidthMM).TypeConverter(new TireWidthConverter());
            Map(m => m.RearProfile).TypeConverter(new TireProfileConverter());
            Map(m => m.RearDiameterInches);
            Map(m => m.RearWidthMM).TypeConverter(new TireWidthConverter());
        }
    }
}