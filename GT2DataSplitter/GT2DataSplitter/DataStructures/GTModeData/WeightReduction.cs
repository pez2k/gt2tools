using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2DataSplitter
{
    public class WeightReduction : CarCsvDataStructure<WeightReductionData, WeightReductionCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
    public struct WeightReductionData
    {
        public uint CarId;
        public uint Price;
        public ushort Weight;
        public byte Unknown;
        public byte Stage;
    }

    public sealed class WeightReductionCSVMap : ClassMap<WeightReductionData>
    {
        public WeightReductionCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Weight);
            Map(m => m.Unknown);
            Map(m => m.Stage);
        }
    }
}
