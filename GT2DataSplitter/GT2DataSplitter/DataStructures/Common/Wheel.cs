using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using StreamExtensions;
    using TypeConverters;

    public class Wheel : CarCsvDataStructure<WheelData, WheelCSVMap>
    {
        public Wheel() => hasCarId = false;

        protected override string CreateOutputFilename()
        {
            string wheelID = new WheelIdConverter().ConvertToString(rawData.ReadUInt(), null, null);
            return $"{Name}\\{Directory.GetFiles(Name).Length:D3}_{(string.IsNullOrEmpty(wheelID) ? "None" : wheelID)}.csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x08, maybe
    public struct WheelData
    {
        public uint WheelId;
        public byte StageMaybe; // always 0 for unnamed, 1 for named
        public byte Unknown; // always 0
        public byte Unknown2; // always 1 for named, 0-3 for unnamed
        public byte Unknown3; // always 2 for named, 0-3 for unnamed
    }

    public sealed class WheelCSVMap : ClassMap<WheelData>
    {
        public WheelCSVMap()
        {
            Map(m => m.WheelId).TypeConverter(new WheelIdConverter());
            Map(m => m.StageMaybe);
            Map(m => m.Unknown);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
        }
    }
}