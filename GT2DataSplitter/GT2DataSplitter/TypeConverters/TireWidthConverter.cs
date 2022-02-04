using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class TireWidthConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)(ushort.Parse(text) / 10);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return $"{(((byte)value) * 10) + 5}";
        }
    }
}