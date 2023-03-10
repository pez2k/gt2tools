using System;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class StringTableLookup : ITypeConverter
    {
        private readonly List<string> table;

        public StringTableLookup(List<string> table) => this.table = table;

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => throw new NotImplementedException();

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => table[int.Parse(value.ToString())];
    }
}