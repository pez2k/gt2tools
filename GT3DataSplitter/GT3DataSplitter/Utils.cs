using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT3.DataSplitter
{
    public static class Utils
    {
        public static IdConverter IdConverter { get; set; } = new IdConverter();
    }

    public class IdConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => Program.IDStrings.Add(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => Program.IDStrings.Get((ulong)value);
    }
}
