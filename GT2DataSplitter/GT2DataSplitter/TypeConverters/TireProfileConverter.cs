using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class TireProfileConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)(ushort.Parse(text) / 5);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return $"{((byte)value) * 5}";
        }
    }
}