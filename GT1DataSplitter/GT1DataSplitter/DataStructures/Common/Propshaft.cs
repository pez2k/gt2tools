using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class Propshaft : CsvDataStructure<PropshaftData, PropshaftCSVMap>
    {
        public Propshaft()
        {
            Header = "PRPSHFT";
            StringTableCount = 2;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct PropshaftData
    {
        public byte Unknown;
        public byte Unknown2;
        public byte Unknown3;
        public byte Padding;
        public ushort CarID;
        public ushort Padding2;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class PropshaftCSVMap : ClassMap<PropshaftData>
    {
        public PropshaftCSVMap(List<List<string>> tables)
        {
            Map(m => m.Unknown);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.CarID);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}