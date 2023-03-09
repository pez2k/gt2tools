using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class DisplacementStringConverter : ITypeConverter
    {
        private const ushort TimesThree = 0x6000;
        private const ushort TimesTwo = 0x4000;

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string[] parts = text.Split('x');
            return parts.Length > 1 ? (ushort)(ushort.Parse(parts[0]) + ParseSuffix(parts[1]))
                                    : ushort.Parse(text);
        }

        private static ushort ParseSuffix(string text) =>
            text == "3" ? TimesThree : text == "2" ? TimesTwo : (ushort)0;

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            ushort displacement = (ushort)value;
            string suffix = "";
            if (displacement > TimesThree)
            {
                displacement -= TimesThree;
                suffix = "x3";
            }
            else if (displacement > TimesTwo)
            {
                displacement -= TimesTwo;
                suffix = "x2";
            }
            return $"{displacement}{suffix}";
        }
    }
}