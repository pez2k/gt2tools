using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2DataSplitter
{
    public class Clutch : CarCsvDataStructure<ClutchData, ClutchCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
    public struct ClutchData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte RPMDropRate;
        public byte InertiaDisengaged;
        public byte InertiaEngaged;
        public byte InertialWeight;
        public byte InertiaBraking;
        public byte Unknown1;
        public byte Unknown2;
    }

    public sealed class ClutchCSVMap : ClassMap<ClutchData>
    {
        public ClutchCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.RPMDropRate);
            Map(m => m.InertiaDisengaged);
            Map(m => m.InertiaEngaged);
            Map(m => m.InertialWeight);
            Map(m => m.InertiaBraking);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
        }
    }
}
