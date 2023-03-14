using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class Tire : CsvDataStructure<TireData, TireCSVMap>
    {
        public Tire()
        {
            Header = "TIRE";
            StringTableCount = 2;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct TireData
    {
        public ushort Compound;
        public ushort Size;
        public ushort CarID;
        public sbyte Stage;
        public sbyte IsBuyableMaybe;
        public uint Price;
        public ushort NamePart1;
        public ushort Padding3;
        public ushort NamePart2;
        public ushort UnknownAlways1;
    }

    public sealed class TireCSVMap : ClassMap<TireData>
    {
        public TireCSVMap(List<List<string>> tables)
        {
            Map(m => m.Compound).PartFilename(nameof(TireCompound));
            Map(m => m.Size).PartFilename(nameof(TireSize));
            Map(m => m.CarID);
            Map(m => m.Stage);
            Map(m => m.IsBuyableMaybe);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.UnknownAlways1);
        }
    }
}