using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using Caches;
    using TypeConverters;

    public class RacingModify : CsvDataStructure<RacingModifyData, RacingModifyCSVMap>
    {
        public RacingModify()
        {
            Header = "RACING";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{CarIDCache.Get(data.CarID)}{Path.GetExtension(filename)}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x20
    public struct RacingModifyData
    {
        public ushort FrontTrackMM;
        public ushort RearTrackMM;
        public ushort WidthMM;
        public byte Unknown;
        public byte WeightPercentage;
        public byte FrontDownforceDefault;
        public byte RearDownforceDefault;
        public byte FrontDownforceMin;
        public byte FrontDownforceMax;
        public byte RearDownforceMin;
        public byte RearDownforceMax;
        public ushort CarID;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
        public uint Padding3;
    }

    public sealed class RacingModifyCSVMap : ClassMap<RacingModifyData>
    {
        public RacingModifyCSVMap(List<List<string>> tables)
        {
            Map(m => m.FrontTrackMM);
            Map(m => m.RearTrackMM);
            Map(m => m.WidthMM);
            Map(m => m.WeightPercentage);
            Map(m => m.Unknown);
            Map(m => m.FrontDownforceDefault);
            Map(m => m.RearDownforceDefault);
            Map(m => m.FrontDownforceMin);
            Map(m => m.FrontDownforceMax);
            Map(m => m.RearDownforceMin);
            Map(m => m.RearDownforceMax);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}