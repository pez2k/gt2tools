using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class TireSize : CsvDataStructure<TireSizeData, TireSizeCSVMap>
    {
        public TireSize()
        {
            CacheFilename = true;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            string filename = base.CreateOutputFilename(data);
            return Path.Combine(Path.GetDirectoryName(filename),
                                $"{Path.GetFileNameWithoutExtension(filename).Substring(1)}_{Utils.TireWidthConverter.ConvertToString(Data.WidthMM, null, null)}" +
                                $"-{Utils.TireProfileConverter.ConvertToString(Data.Profile, null, null)}R{Data.DiameterInches}{Path.GetExtension(filename)}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x04
    public struct TireSizeData
    {
        public byte DiameterInches;
        public byte WidthMM;
        public ushort Profile;
    }

    public sealed class TireSizeCSVMap : ClassMap<TireSizeData>
    {
        public TireSizeCSVMap()
        {
            Map(m => m.DiameterInches);
            Map(m => m.WidthMM).TypeConverter(Utils.TireWidthConverter);
            Map(m => m.Profile).TypeConverter(Utils.TireProfileConverter);
        }
    }
}
