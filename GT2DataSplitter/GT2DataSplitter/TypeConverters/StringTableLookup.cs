using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class StringTableLookup : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => StringTable.Add(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => StringTable.Get(Convert.ToUInt16(value));
    }
}