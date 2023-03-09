using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class BackwardCompatibleShortConverter : Int16Converter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            NumberStyles numberStyle = memberMapData.TypeConverterOptions.NumberStyles ?? NumberStyles.Integer;
            return short.TryParse(text, numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out short signedValue)
                ? signedValue
                : ushort.TryParse(text, numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out ushort legacyUnsignedValue)
                    ? (short)(legacyUnsignedValue - ushort.MaxValue - 1)
                    : base.ConvertFromString(text, row, memberMapData);
        }
    }
}