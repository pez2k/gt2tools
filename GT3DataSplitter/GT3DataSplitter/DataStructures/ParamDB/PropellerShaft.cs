using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class PropellerShaft : CsvDataStructure<PropellerShaftData, PropellerShaftCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x20
    public struct PropellerShaftData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte EngineBraking;
        public byte IWheelF;
        public byte IWheelR;
        public byte IPropF;
        public byte IPropR;
        public ushort Unknown1;
        public uint Unknown2;
        public uint Price;
    }

    public sealed class PropellerShaftCSVMap : ClassMap<PropellerShaftData>
    {
        public PropellerShaftCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.EngineBraking);
            Map(m => m.IWheelF);
            Map(m => m.IWheelR);
            Map(m => m.IPropF);
            Map(m => m.IPropR);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Price);
        }
    }
}