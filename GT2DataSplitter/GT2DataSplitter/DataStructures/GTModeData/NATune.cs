using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class NATune : CarCsvDataStructure<NATuneData, NATuneCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x0C
    public struct NATuneData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public sbyte PowerbandRPMIncrease;
        public sbyte RPMIncrease;
        public byte PowerMultiplier;
    }

    public sealed class NATuneCSVMap : ClassMap<NATuneData>
    {
        public NATuneCSVMap()
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