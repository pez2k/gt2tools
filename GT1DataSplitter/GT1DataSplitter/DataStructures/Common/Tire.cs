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
            cacheFilename = true;
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
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class TireCSVMap : ClassMap<TireData>
    {
        public TireCSVMap(List<List<string>> tables)
        {
            Map(m => m.Compound).PartFilename(nameof(TireCompound));
            Map(m => m.Size).PartFilename(nameof(TireSize));
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Stage);
            Map(m => m.IsBuyableMaybe);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}