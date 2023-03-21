using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public abstract class Stabilizer : CsvDataStructure<StabilizerData, StabilizerCSVMap>
    {
        public Stabilizer()
        {
            Header = "STABILZ";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct StabilizerData
    {
        public byte Default; // kgf/m^2 / 10
        public byte Steps;
        public byte Min;
        public byte Max;
        public ushort CarID;
        public byte Stage;
        public byte StageDuplicate;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class StabilizerCSVMap : ClassMap<StabilizerData>
    {
        public StabilizerCSVMap(List<List<string>> tables)
        {
            Map(m => m.Default);
            Map(m => m.Steps);
            Map(m => m.Min);
            Map(m => m.Max);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Stage);
            Map(m => m.StageDuplicate);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}