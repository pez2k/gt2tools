using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2DataSplitter
{
    public class Dimensions : CarCsvDataStructure<DimensionsData, DimensionsCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Utils.GetCarName(Data.CarId) + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct DimensionsData
    {
        public uint CarId;
        public byte Unknown;
        public byte Unknown2;
        public byte FrontGrip;
        public byte RearGrip;
        public ushort Length;
        public ushort Height;
        public ushort LongitudinalWeightTransfer;
        public ushort Weight;
        public byte RMWeightMultiplier;
        public byte Unknown6;
        public byte Unknown7;
        public byte Unknown8;
    }

    public sealed class DimensionsCSVMap : ClassMap<DimensionsData>
    {
        public DimensionsCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Unknown);
            Map(m => m.Unknown2);
            Map(m => m.FrontGrip);
            Map(m => m.RearGrip);
            Map(m => m.Length);
            Map(m => m.Height);
            Map(m => m.LongitudinalWeightTransfer);
            Map(m => m.Weight);
            Map(m => m.RMWeightMultiplier);
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.Unknown8);
        }
    }
}
