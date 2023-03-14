using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class HexStringConverter : ITypeConverter
    {
        private readonly int expectedLength;

        public HexStringConverter(int expectedLength) => this.expectedLength = expectedLength;

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            byte[] array = Encoding.ASCII.GetBytes(text);
            return array.Concat(Enumerable.Range(0, expectedLength - array.Length).Select(i => (byte)0)).ToArray();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => Encoding.ASCII.GetString((byte[])value).TrimEnd('\0');
    }
}