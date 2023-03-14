using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class BrakeController : CsvDataStructure<BrakeControllerData, BrakeControllerCSVMap>
    {
        public BrakeController()
        {
            Header = "BRKCTRL";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0xA);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x1C
    public struct BrakeControllerData
    {
        public byte FrontDefault;
        public byte RearDefault;
        public byte FrontMin;
        public byte FrontMax;
        public byte FrontSteps;
        public byte RearMin;
        public byte RearMax;
        public byte RearSteps;
        public ushort Padding;
        public ushort CarID;
        public uint Padding2;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class BrakeControllerCSVMap : ClassMap<BrakeControllerData>
    {
        public BrakeControllerCSVMap(List<List<string>> tables)
        {
            Map(m => m.FrontDefault);
            Map(m => m.RearDefault);
            Map(m => m.FrontMin);
            Map(m => m.FrontMax);
            Map(m => m.FrontSteps);
            Map(m => m.RearMin);
            Map(m => m.RearMax);
            Map(m => m.RearSteps);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}