using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    public class GenericEngineUpgrade : CarCsvDataStructure<GenericEngineUpgradeData, GenericEngineUpgradeCSVMap>
    {
        protected override string CreateOutputFilename() => CreateOutputFilename(data.CarId, data.Stage);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
    public struct GenericEngineUpgradeData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public sbyte PowerbandScaling;
        public sbyte RPMIncrease;
        public byte PowerMultiplier;
    }

    public sealed class GenericEngineUpgradeCSVMap : ClassMap<GenericEngineUpgradeData>
    {
        public GenericEngineUpgradeCSVMap()
        {
            Map(m => m.CarId).CarId();
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.PowerbandScaling);
            Map(m => m.RPMIncrease);
            Map(m => m.PowerMultiplier);
        }
    }
}