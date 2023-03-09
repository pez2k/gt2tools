using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class ASCIIStringTableLookup : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => ASCIIStringTable.Add(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => ASCIIStringTable.Get(Convert.ToUInt16(value));
    }
}