using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Brake : CsvDataStructure<BrakeData, BrakeCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x18
    public struct BrakeData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte FrontBrakeTorque;
        public byte RearBrakeTorque;
        public byte HandbrakeTorque;
        public uint Price;
    }

    public sealed class BrakeCSVMap : ClassMap<BrakeData>
    {
        public BrakeCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.FrontBrakeTorque);
            Map(m => m.RearBrakeTorque);
            Map(m => m.HandbrakeTorque);
            Map(m => m.Price);
        }
    }
}