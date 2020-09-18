using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Chassis : CsvDataStructure<ChassisData, ChassisCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x20
    public struct ChassisData
    {
        public ulong Part;
        public ulong Car;
        public byte FrontWeightDistribution;
        public byte AdjustableDownforce;
        public ushort Unknown;
        public ushort Length;
        public ushort Height;
        public ushort Wheelbase;
        public ushort Weight;
        public byte Unknown2;
        public byte Unknown3;
        public ushort Unknown4;
    }

    public sealed class ChassisCSVMap : ClassMap<ChassisData>
    {
        public ChassisCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.FrontWeightDistribution);
            Map(m => m.AdjustableDownforce);
            Map(m => m.Unknown);
            Map(m => m.Length);
            Map(m => m.Height);
            Map(m => m.Wheelbase);
            Map(m => m.Weight);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
        }
    }
}