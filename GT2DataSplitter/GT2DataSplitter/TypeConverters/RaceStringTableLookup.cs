using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class RaceStringTableLookup : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => RaceStringTable.Add(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => RaceStringTable.Get(Convert.ToUInt16(value));
    }
}