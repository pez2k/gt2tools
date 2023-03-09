using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class TireWidthConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => (byte)(ushort.Parse(text) / 10);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => $"{(((byte)value) * 10) + 5}";
    }
}