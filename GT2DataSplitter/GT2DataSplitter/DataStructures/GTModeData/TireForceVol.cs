using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class TireForceVol : CsvDataStructure<TireForceVolData, TireForceVolCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x08
    public struct TireForceVolData
    {
        public byte TarmacGrip;
        public byte GutterGrip;
        public byte GrassGrip;
        public byte SandGrip;
        public byte GravelGrip;
        public byte DirtGrip;
        public byte WetGripMaybe;
        public byte Padding;
    }

    public sealed class TireForceVolCSVMap : ClassMap<TireForceVolData>
    {
        public TireForceVolCSVMap()
        {
            Map(m => m.TarmacGrip);
            Map(m => m.GutterGrip);
            Map(m => m.GrassGrip);
            Map(m => m.SandGrip);
            Map(m => m.GravelGrip);
            Map(m => m.DirtGrip);
            Map(m => m.WetGripMaybe);
        }
    }
}
