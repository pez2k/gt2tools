using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    public class LSD : CarCsvDataStructure<LSDData, LSDCSVMap>
    {
        protected override string CreateOutputFilename() => CreateOutputFilename(data.CarId, data.Stage);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x20
    public struct LSDData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte Unknown;
        public byte Unknown2;
        public byte Unknown3;
        public byte FrontUnknown; // diff type? 102 for null, 109 any upgrade & R34 rear, 118 for GTI, 110 for Cooper & R34 front, 104 for DC2
        public byte DefaultInitialFront;
        public byte MinInitialFront;
        public byte MaxInitialFront;
        public byte DefaultAccelFront;
        public byte MinAccelFront;
        public byte MaxAccelFront;
        public byte DefaultDecelFront;
        public byte MinDecelFront;
        public byte MaxDecelFront;
        public byte RearUnknown; // diff type?
        public byte DefaultInitialRear;
        public byte MinInitialRear;
        public byte MaxInitialRear;
        public byte DefaultAccelRear;
        public byte MinAccelRear;
        public byte MaxAccelRear;
        public byte DefaultDecelRear;
        public byte MinDecelRear;
        public byte MaxDecelRear;
    }

    public sealed class LSDCSVMap : ClassMap<LSDData>
    {
        public LSDCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.Unknown);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.FrontUnknown);
            Map(m => m.DefaultInitialFront);
            Map(m => m.MinInitialFront);
            Map(m => m.MaxInitialFront);
            Map(m => m.DefaultAccelFront);
            Map(m => m.MinAccelFront);
            Map(m => m.MaxAccelFront);
            Map(m => m.DefaultDecelFront);
            Map(m => m.MinDecelFront);
            Map(m => m.MaxDecelFront);
            Map(m => m.RearUnknown);
            Map(m => m.DefaultInitialRear);
            Map(m => m.MinInitialRear);
            Map(m => m.MaxInitialRear);
            Map(m => m.DefaultAccelRear);
            Map(m => m.MinAccelRear);
            Map(m => m.MaxAccelRear);
            Map(m => m.DefaultDecelRear);
            Map(m => m.MinDecelRear);
            Map(m => m.MaxDecelRear);
        }
    }
}