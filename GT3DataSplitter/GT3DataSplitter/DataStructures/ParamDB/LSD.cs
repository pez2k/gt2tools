using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class LSD : CsvDataStructure<LSDData, LSDCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x30
    public struct LSDData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte DiffTypeFront; // 102 for none
        public byte DefaultInitialFront;
        public byte MinInitialFront;
        public byte MaxInitialFront;
        public byte DefaultAccelFront;
        public byte MinAccelFront;
        public byte MaxAccelFront;
        public byte DefaultDecelFront;
        public byte MinDecelFront;
        public byte MaxDecelFront;
        public byte DiffTypeRear;
        public byte DefaultInitialRear;
        public byte MinInitialRear;
        public byte MaxInitialRear;
        public byte DefaultAccelRear;
        public byte MinAccelRear;
        public byte MaxAccelRear;
        public byte DefaultDecelRear;
        public byte MinDecelRear;
        public byte MaxDecelRear;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] Padding;
        public uint Price;
    }

    public sealed class LSDCSVMap : ClassMap<LSDData>
    {
        public LSDCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.DiffTypeFront);
            Map(m => m.DefaultInitialFront);
            Map(m => m.MinInitialFront);
            Map(m => m.MaxInitialFront);
            Map(m => m.DefaultAccelFront);
            Map(m => m.MinAccelFront);
            Map(m => m.MaxAccelFront);
            Map(m => m.DefaultDecelFront);
            Map(m => m.MinDecelFront);
            Map(m => m.MaxDecelFront);
            Map(m => m.DiffTypeRear);
            Map(m => m.DefaultInitialRear);
            Map(m => m.MinInitialRear);
            Map(m => m.MaxInitialRear);
            Map(m => m.DefaultAccelRear);
            Map(m => m.MinAccelRear);
            Map(m => m.MaxAccelRear);
            Map(m => m.DefaultDecelRear);
            Map(m => m.MinDecelRear);
            Map(m => m.MaxDecelRear);
            Map(m => m.Price);
        }
    }
}