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

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            int existingEntry = table.IndexOf(text);
            if (existingEntry >= 0)
            {
                return (ushort)existingEntry;
            }
            table.Add(text);
            return (ushort)(table.Count - 1);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => table[int.Parse(value.ToString())];
    }
}