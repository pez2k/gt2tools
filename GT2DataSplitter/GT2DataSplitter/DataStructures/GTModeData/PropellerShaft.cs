using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class PropellerShaft : CarCsvDataStructure<PropellerShaftData, PropellerShaftCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
    public struct PropellerShaftData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte RPMDropRate;
        public byte Inertia;
        public byte Inertia2;
    }

    public sealed class PropellerShaftCSVMap : ClassMap<PropellerShaftData>
    {
        public PropellerShaftCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.RPMDropRate);
            Map(m => m.Inertia);
            Map(m => m.Inertia2);
        }
    }
}
