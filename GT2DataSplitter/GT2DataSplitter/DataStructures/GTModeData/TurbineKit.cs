using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    public class TurbineKit : CarCsvDataStructure<TurbineKitData, TurbineKitCSVMap>
    {
        protected override string CreateOutputFilename() => CreateOutputFilename(data.CarId, data.Stage);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct TurbineKitData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte BoostGaugeLimit;
        public byte LowRPMBoost;
        public byte HighRPMBoost;
        public byte SpoolRate;
        public byte Unknown1;
        public byte Unknown2;
        public byte Unknown3;
        public sbyte RPMIncrease;
        public sbyte RedlineIncrease;
        public byte HighRPMPowerMultiplier;
        public byte LowRPMPowerMultiplier;
    }

    public sealed class TurbineKitCSVMap : ClassMap<TurbineKitData>
    {
        public TurbineKitCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.BoostGaugeLimit);
            Map(m => m.LowRPMBoost);
            Map(m => m.HighRPMBoost);
            Map(m => m.SpoolRate);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.RPMIncrease);
            Map(m => m.RedlineIncrease);
            Map(m => m.HighRPMPowerMultiplier);
            Map(m => m.LowRPMPowerMultiplier);
        }
    }
}