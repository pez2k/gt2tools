using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class Dimensions : CarCsvDataStructure<DimensionsData, DimensionsCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Data.CarId.ToCarName() + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct DimensionsData
    {
        public uint CarId;
        public byte CentreOfMassLongitudinal;
        public byte Unknown2;
        public byte FrontGrip;
        public byte RearGrip;
        public ushort Length;
        public ushort Height;
        public ushort CentreOfMassHeight;
        public ushort Weight;
        public byte TurningResistance;
        public byte PitchResistance;
        public byte RollResistance;
        public byte Unknown8;
    }

    public sealed class DimensionsCSVMap : ClassMap<DimensionsData>
    {
        public DimensionsCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.CentreOfMassLongitudinal);
            Map(m => m.Unknown2);
            Map(m => m.FrontGrip);
            Map(m => m.RearGrip);
            Map(m => m.Length);
            Map(m => m.Height);
            Map(m => m.CentreOfMassHeight);
            Map(m => m.Weight);
            Map(m => m.TurningResistance);
            Map(m => m.PitchResistance);
            Map(m => m.RollResistance);
            Map(m => m.Unknown8);
        }
    }
}
