using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    using Caches;

    public class CachedFileNameConverter : ITypeConverter
    {
        private readonly string name;

        public CachedFileNameConverter(string name) => this.name = name;

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            int stringNumber = FileNameCache.Get(name, text);
            return (ushort)(stringNumber + 1);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) =>
            FileNameCache.Get(name, int.Parse(value.ToString()) - 1);
    }
}