using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class UnicodeStringTableLookup : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => UnicodeStringTable.Add(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => UnicodeStringTable.Get(Convert.ToUInt16(value));
    }
}