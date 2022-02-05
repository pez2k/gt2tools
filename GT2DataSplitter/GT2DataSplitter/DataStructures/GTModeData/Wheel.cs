using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    public class Wheel : CarCsvDataStructure<WheelData, WheelCSVMap>
    {
        public Wheel() => hasCarId = false;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x08, maybe
    public struct WheelData
    {
        public uint WheelId; // manufacturer ID byte, wheel number byte, lug count enum byte, colour ASCII char - eg 70 07 40 73 for Speedline 07 5-lug silver / sp007-5s - 00 / 20 / 40 / 60 for - / 4 / 5 / 6 lugs
        public byte Unknown2;
        public byte Unknown3;
        public byte Unknown4;
        public byte Unknown5;
    }

    public sealed class WheelCSVMap : ClassMap<WheelData>
    {
        public WheelCSVMap()
        {
            Map(m => m.WheelId);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Unknown5);
        }
    }
}