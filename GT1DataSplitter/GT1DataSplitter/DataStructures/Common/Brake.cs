using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class Brake : CsvDataStructure<BrakeData, BrakeCSVMap>
    {
        public Brake()
        {
            Header = "BRAKE";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x18
    public struct BrakeData
    {
        public byte FrontBrakeTorqueKGFM;
        public byte RearBrakeTorqueKGFM;
        public byte Unknown;
        public byte Padding;
        public ushort CarID;
        public ushort Padding2;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
        public uint Padding4;
    }

    public sealed class BrakeCSVMap : ClassMap<BrakeData>
    {
        public BrakeCSVMap(List<List<string>> tables)
        {
            Map(m => m.FrontBrakeTorqueKGFM);
            Map(m => m.RearBrakeTorqueKGFM);
            Map(m => m.Unknown);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}