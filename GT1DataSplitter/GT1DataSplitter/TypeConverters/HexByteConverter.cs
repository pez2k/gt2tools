using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class HexByteConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => byte.Parse(text, NumberStyles.HexNumber);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => $"{value:X2}";
    }
}