using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2DataSplitter
{
    public class BrakeBalanceController : CarCsvDataStructure<BrakeBalanceControllerData, BrakeBalanceControllerCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
    public struct BrakeBalanceControllerData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte MaxFrontBias;
        public byte Unknown;
        public byte Unknown2;
        public byte DefaultBias;
        public byte MaxRearBias;
        public byte Unknown3;
        public byte Unknown4;
    }

    public sealed class BrakeBalanceControllerCSVMap : ClassMap<BrakeBalanceControllerData>
    {
        public BrakeBalanceControllerCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.MaxFrontBias);
            Map(m => m.Unknown);
            Map(m => m.Unknown2);
            Map(m => m.DefaultBias);
            Map(m => m.MaxRearBias);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
        }
    }
}
