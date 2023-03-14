using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    using Caches;

    public class CachedCarIDConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => (ushort)CarIDCache.Get(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => CarIDCache.Get(int.Parse(value.ToString()));
    }
}