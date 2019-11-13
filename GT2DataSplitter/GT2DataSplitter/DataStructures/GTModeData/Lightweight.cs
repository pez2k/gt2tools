using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class Lightweight : CarCsvDataStructure<LightweightData, LightweightCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
    public struct LightweightData
    {
        public uint CarId;
        public uint Price;
        public ushort Weight;
        public byte Unknown;
        public byte Stage;
    }

    public sealed class LightweightCSVMap : ClassMap<LightweightData>
    {
        public LightweightCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Weight);
            Map(m => m.Unknown);
            Map(m => m.Stage);
        }
    }
}
