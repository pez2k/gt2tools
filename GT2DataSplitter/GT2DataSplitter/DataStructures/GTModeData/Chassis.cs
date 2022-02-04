using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class Chassis : CarCsvDataStructure<ChassisData, ChassisCSVMap>
    {
        protected override string CreateOutputFilename() => Name + "\\" + data.CarId.ToCarName() + ".csv";
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct ChassisData
    {
        public uint CarId;
        public byte CentreOfMassLongitudinal; // weight distribution %
        public byte Unknown2;
        public byte FrontGrip;
        public byte RearGrip;
        public ushort Length;
        public ushort Height;
        public ushort CentreOfMassHeight; // wheelbase mm
        public ushort Weight;
        public byte TurningResistance;
        public byte PitchResistance;
        public byte RollResistance;
        public byte Unknown8;
    }

    public sealed class ChassisCSVMap : ClassMap<ChassisData>
    {
        public ChassisCSVMap()
        {
            Map(m => m.CarId).CarId();
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