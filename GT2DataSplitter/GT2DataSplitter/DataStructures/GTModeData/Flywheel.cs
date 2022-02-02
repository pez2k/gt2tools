using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    public class Flywheel : CarCsvDataStructure<FlywheelData, FlywheelCSVMap>
    {
        protected override string CreateOutputFilename() => CreateOutputFilename(data.CarId, data.Stage);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
    public struct FlywheelData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte RPMDropRate;
        public byte ShiftDelay;
        public byte InertialWeight;
    }

    public sealed class FlywheelCSVMap : ClassMap<FlywheelData>
    {
        public FlywheelCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.RPMDropRate);
            Map(m => m.ShiftDelay);
            Map(m => m.InertialWeight);
        }
    }
}