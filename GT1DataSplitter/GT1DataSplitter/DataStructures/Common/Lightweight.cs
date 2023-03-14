using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using Caches;
    using TypeConverters;

    public class Lightweight : CsvDataStructure<LightweightData, LightweightCSVMap>
    {
        public Lightweight()
        {
            Header = "LWEIGHT";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{CarIDCache.Get(data.CarID)}_stage{data.Stage + 1:X2}{Path.GetExtension(filename)}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct LightweightData
    {
        public byte WeightPercentage;
        public byte Stage;
        public ushort CarID;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
        public uint StageDuplicate;
    }

    public sealed class LightweightCSVMap : ClassMap<LightweightData>
    {
        public LightweightCSVMap(List<List<string>> tables)
        {
            Map(m => m.WeightPercentage);
            Map(m => m.Stage);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
            Map(m => m.StageDuplicate);
        }
    }
}