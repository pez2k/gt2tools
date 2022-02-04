using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    using CarNameConversion;

    public class CarIdConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => text.ToCarID();

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => ((uint)value).ToCarName();
    }
}