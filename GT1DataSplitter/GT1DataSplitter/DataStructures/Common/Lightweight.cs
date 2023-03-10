using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class Lightweight : CsvDataStructure<LightweightData, LightweightCSVMap>
    {
        public Lightweight()
        {
            Header = "LWEIGHT";
            StringTableCount = 2;
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename),
                $"_car{rawData[0x2]:X2}" +
                $"_stage{rawData[0x1] + 1:X2}" +
                $"_{Parent.StringTables[0][rawData[0x8]]}" +
                $" {Parent.StringTables[1][rawData[0xC]].Replace('/', '-')}" +
                Path.GetExtension(filename));
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct LightweightData
    {
        public byte WeightPercentage;
        public byte Stage;
        public byte CarID;
        public byte Padding;
        public uint Price;
        public ushort NamePart1;
        public ushort Padding2;
        public ushort NamePart2;
        public ushort UnknownAlways1;
        public uint StageDuplicate;
    }

    public sealed class LightweightCSVMap : ClassMap<LightweightData>
    {
        public LightweightCSVMap(List<List<string>> tables)
        {
            Map(m => m.WeightPercentage);
            Map(m => m.Stage);
            Map(m => m.CarID);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.UnknownAlways1);
            Map(m => m.StageDuplicate);
        }
    }
}