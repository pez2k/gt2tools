using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class NATune : CsvDataStructure<NATuneData, NATuneCSVMap>
    {
        public NATune()
        {
            Header = "NATUNE";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x10);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x24
    public struct NATuneData
    {
        public byte TorqueMultiplier1;
        public byte TorqueMultiplier2;
        public byte TorqueMultiplier3;
        public byte TorqueMultiplier4;
        public byte TorqueMultiplier5;
        public byte TorqueMultiplier6;
        public byte TorqueMultiplier7;
        public byte TorqueMultiplier8;
        public byte TorqueMultiplier9;
        public byte TorqueMultiplier10;
        public byte TorqueMultiplier11;
        public byte TorqueMultiplier12;
        public byte TorqueMultiplier13;
        public byte TorqueMultiplier14;
        public byte TorqueMultiplier15;
        public byte TorqueMultiplier16;
        public ushort CarID;
        public sbyte PowerbandRPMIncrease;
        public byte Stage;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
        public uint StageDuplicate;
    }

    public sealed class NATuneCSVMap : ClassMap<NATuneData>
    {
        public NATuneCSVMap(List<List<string>> tables)
        {
            Map(m => m.TorqueMultiplier1);
            Map(m => m.TorqueMultiplier2);
            Map(m => m.TorqueMultiplier3);
            Map(m => m.TorqueMultiplier4);
            Map(m => m.TorqueMultiplier5);
            Map(m => m.TorqueMultiplier6);
            Map(m => m.TorqueMultiplier7);
            Map(m => m.TorqueMultiplier8);
            Map(m => m.TorqueMultiplier9);
            Map(m => m.TorqueMultiplier10);
            Map(m => m.TorqueMultiplier11);
            Map(m => m.TorqueMultiplier12);
            Map(m => m.TorqueMultiplier13);
            Map(m => m.TorqueMultiplier14);
            Map(m => m.TorqueMultiplier15);
            Map(m => m.TorqueMultiplier16);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.PowerbandRPMIncrease);
            Map(m => m.Stage);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
            Map(m => m.StageDuplicate);
        }
    }
}