using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Clutch : CsvDataStructure<ClutchData, ClutchCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x20
    public struct ClutchData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte EngineBraking;
        public byte FlywheelInertia;
        public byte FrontWheelInertia;
        public byte RearWheelInertia;
        public byte ClutchTorque;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Padding;
        public uint Price;
    }

    public sealed class ClutchCSVMap : ClassMap<ClutchData>
    {
        public ClutchCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.EngineBraking);
            Map(m => m.FlywheelInertia);
            Map(m => m.FrontWheelInertia);
            Map(m => m.RearWheelInertia);
            Map(m => m.ClutchTorque);
            Map(m => m.Price);
        }
    }
}