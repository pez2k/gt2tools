using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Displacement : CsvDataStructure<DisplacementData, DisplacementCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x18
    public struct DisplacementData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte TorqueModifier;
        public byte TorqueModifier2;
        public byte TorqueModifier3;
        public uint Price;
    }

    public sealed class DisplacementCSVMap : ClassMap<DisplacementData>
    {
        public DisplacementCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.TorqueModifier);
            Map(m => m.TorqueModifier2);
            Map(m => m.TorqueModifier3);
            Map(m => m.Price);
        }
    }
}
