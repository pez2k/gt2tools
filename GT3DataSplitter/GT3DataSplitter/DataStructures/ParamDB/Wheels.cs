using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Wheels : CsvDataStructure<WheelsData, WheelsCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
    public struct WheelsData
    {
        public ulong Wheel;
        public ushort Unknown;
        public ushort Padding;
        public ushort Filename;
        public ushort Padding2;
    }

    public sealed class WheelsCSVMap : ClassMap<WheelsData>
    {
        public WheelsCSVMap()
        {
            Map(m => m.Wheel).TypeConverter(Utils.IdConverter);
            Map(m => m.Unknown);
            Map(m => m.Filename).TypeConverter(Program.Strings.Lookup);
        }
    }
}