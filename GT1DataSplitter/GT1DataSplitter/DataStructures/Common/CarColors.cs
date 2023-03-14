using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using Caches;
    using TypeConverters;

    public class CarColors : CsvDataStructure<CarColorsData, CarColorsCSVMap>
    {
        public CarColors()
        {
            Header = "COLOR";
            StringTableCount = 16;
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{CarIDCache.Get(data.CarID)}{Path.GetExtension(filename)}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x54
    public struct CarColorsData
    {
        public ushort CarID;
        public byte ColorID1;
        public byte ColorID2;
        public byte ColorID3;
        public byte ColorID4;
        public byte ColorID5;
        public byte ColorID6;
        public byte ColorID7;
        public byte ColorID8;
        public byte ColorID9;
        public byte ColorID10;
        public byte ColorID11;
        public byte ColorID12;
        public byte ColorID13;
        public byte ColorID14;
        public byte ColorID15;
        public byte ColorID16;
        public ushort Padding;
        public ushort ColorName1;
        public ushort ColorStringTable1;
        public ushort ColorName2;
        public ushort ColorStringTable2;
        public ushort ColorName3;
        public ushort ColorStringTable3;
        public ushort ColorName4;
        public ushort ColorStringTable4;
        public ushort ColorName5;
        public ushort ColorStringTable5;
        public ushort ColorName6;
        public ushort ColorStringTable6;
        public ushort ColorName7;
        public ushort ColorStringTable7;
        public ushort ColorName8;
        public ushort ColorStringTable8;
        public ushort ColorName9;
        public ushort ColorStringTable9;
        public ushort ColorName10;
        public ushort ColorStringTable10;
        public ushort ColorName11;
        public ushort ColorStringTable11;
        public ushort ColorName12;
        public ushort ColorStringTable12;
        public ushort ColorName13;
        public ushort ColorStringTable13;
        public ushort ColorName14;
        public ushort ColorStringTable14;
        public ushort ColorName15;
        public ushort ColorStringTable15;
        public ushort ColorName16;
        public ushort ColorStringTable16;
    }

    public sealed class CarColorsCSVMap : ClassMap<CarColorsData>
    {
        public CarColorsCSVMap(List<List<string>> tables)
        {
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.ColorID1).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.ColorStringTable1).Convert(args => 0).Ignore();
            Map(m => m.ColorID2).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.ColorStringTable2).Convert(args => 1).Ignore();
            Map(m => m.ColorID3).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName3).TypeConverter(new StringTableLookup(tables[2]));
            Map(m => m.ColorStringTable3).Convert(args => 2).Ignore();
            Map(m => m.ColorID4).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName4).TypeConverter(new StringTableLookup(tables[3]));
            Map(m => m.ColorStringTable4).Convert(args => 3).Ignore();
            Map(m => m.ColorID5).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName5).TypeConverter(new StringTableLookup(tables[4]));
            Map(m => m.ColorStringTable5).Convert(args => 4).Ignore();
            Map(m => m.ColorID6).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName6).TypeConverter(new StringTableLookup(tables[5]));
            Map(m => m.ColorStringTable6).Convert(args => 5).Ignore();
            Map(m => m.ColorID7).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName7).TypeConverter(new StringTableLookup(tables[6]));
            Map(m => m.ColorStringTable7).Convert(args => 6).Ignore();
            Map(m => m.ColorID8).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName8).TypeConverter(new StringTableLookup(tables[7]));
            Map(m => m.ColorStringTable8).Convert(args => 7).Ignore();
            Map(m => m.ColorID9).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName9).TypeConverter(new StringTableLookup(tables[8]));
            Map(m => m.ColorStringTable9).Convert(args => 8).Ignore();
            Map(m => m.ColorID10).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName10).TypeConverter(new StringTableLookup(tables[9]));
            Map(m => m.ColorStringTable10).Convert(args => 9).Ignore();
            Map(m => m.ColorID11).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName11).TypeConverter(new StringTableLookup(tables[10]));
            Map(m => m.ColorStringTable11).Convert(args => 10).Ignore();
            Map(m => m.ColorID12).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName12).TypeConverter(new StringTableLookup(tables[11]));
            Map(m => m.ColorStringTable12).Convert(args => 11).Ignore();
            Map(m => m.ColorID13).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName13).TypeConverter(new StringTableLookup(tables[12]));
            Map(m => m.ColorStringTable13).Convert(args => 12).Ignore();
            Map(m => m.ColorID14).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName14).TypeConverter(new StringTableLookup(tables[13]));
            Map(m => m.ColorStringTable14).Convert(args => 13).Ignore();
            Map(m => m.ColorID15).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName15).TypeConverter(new StringTableLookup(tables[14]));
            Map(m => m.ColorStringTable15).Convert(args => 14).Ignore();
            Map(m => m.ColorID16).TypeConverter(new HexByteConverter());
            Map(m => m.ColorName16).TypeConverter(new StringTableLookup(tables[15]));
            Map(m => m.ColorStringTable16).Convert(args => 15).Ignore();
        }
    }
}