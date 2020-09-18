using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Lightweight : CsvDataStructure<LightweightData, LightweightCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x18
    public struct LightweightData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte YawEffect;
        public ushort WeightEffect;
        public uint Price;
    }

    public sealed class LightweightCSVMap : ClassMap<LightweightData>
    {
        public LightweightCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.YawEffect);
            Map(m => m.WeightEffect);
            Map(m => m.Price);
        }
    }
}