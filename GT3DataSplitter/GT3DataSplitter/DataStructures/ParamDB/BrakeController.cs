using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class BrakeController : CsvDataStructure<BrakeControllerData, BrakeControllerCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x20
    public struct BrakeControllerData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte FrontSteps; // 24 steps
        public byte FrontMinValue; // from value 10 for step 1
        public byte FrontMaxValue; // to value 240 for step 24
        public byte FrontDefaultStep; // default step is 9
        public byte RearSteps;
        public byte RearMinValue;
        public byte RearMaxValue;
        public byte RearDefaultStep;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Padding;
        public uint Price;
    }

    public sealed class BrakeControllerCSVMap : ClassMap<BrakeControllerData>
    {
        public BrakeControllerCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.FrontSteps);
            Map(m => m.FrontMinValue);
            Map(m => m.FrontMaxValue);
            Map(m => m.FrontDefaultStep);
            Map(m => m.RearSteps);
            Map(m => m.RearMinValue);
            Map(m => m.RearMaxValue);
            Map(m => m.RearDefaultStep);
            Map(m => m.Price);
        }
    }
}