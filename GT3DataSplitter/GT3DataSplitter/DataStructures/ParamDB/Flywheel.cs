using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Flywheel : CsvDataStructure<FlywheelData, FlywheelCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x20
    public struct FlywheelData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte EngineBraking;
        public byte FlywheelInertia;
        public byte FrontWheelInertia;
        public byte RearWheelInertia;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] Padding;
        public uint Price;
    }

    public sealed class FlywheelCSVMap : ClassMap<FlywheelData>
    {
        public FlywheelCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.EngineBraking);
            Map(m => m.FlywheelInertia);
            Map(m => m.FrontWheelInertia);
            Map(m => m.RearWheelInertia);
            Map(m => m.Price);
        }
    }
}