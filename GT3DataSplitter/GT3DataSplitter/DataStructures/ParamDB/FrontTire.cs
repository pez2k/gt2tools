using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class FrontTire : CsvDataStructure<FrontTireData, FrontTireCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x60
    public struct FrontTireData
    {
        public ulong Part;
        public ulong Car;
        public uint Stage;
        public uint Price;
        public ulong Size;
        public ulong Compound1;
        public ulong Compound2;
        public ulong Compound3;
        public ulong Compound4;
        public ulong ForceVol1;
        public ulong ForceVol2;
        public ulong ForceVol3;
        public ulong ForceVol4;
    }

    public sealed class FrontTireCSVMap : ClassMap<FrontTireData>
    {
        public FrontTireCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.Price);
            Map(m => m.Size).TypeConverter(Utils.IdConverter);
            Map(m => m.Compound1).TypeConverter(Utils.IdConverter);
            Map(m => m.Compound2).TypeConverter(Utils.IdConverter);
            Map(m => m.Compound3).TypeConverter(Utils.IdConverter);
            Map(m => m.Compound4).TypeConverter(Utils.IdConverter);
            Map(m => m.ForceVol1).TypeConverter(Utils.IdConverter);
            Map(m => m.ForceVol2).TypeConverter(Utils.IdConverter);
            Map(m => m.ForceVol3).TypeConverter(Utils.IdConverter);
            Map(m => m.ForceVol4).TypeConverter(Utils.IdConverter);
        }
    }
}