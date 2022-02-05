using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    public class NATune : CarCsvDataStructure<NATuneData, NATuneCSVMap>
    {
        protected override string CreateOutputFilename() => CreateOutputFilename(data.CarId, data.Stage);
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
            Map(m => m.CarId).CarId();
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.PowerbandRPMIncrease);
            Map(m => m.RPMIncrease);
            Map(m => m.PowerMultiplier);
        }
    }
}