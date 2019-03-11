using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class NATuning : CarCsvDataStructure<NATuningData, NATuningCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x0C
    public struct NATuningData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public sbyte PowerbandRPMIncrease;
        public sbyte RPMIncrease;
        public byte PowerMultiplier;
    }

    public sealed class NATuningCSVMap : ClassMap<NATuningData>
    {
        public NATuningCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.PowerbandRPMIncrease);
            Map(m => m.RPMIncrease);
            Map(m => m.PowerMultiplier);
        }
    }
}