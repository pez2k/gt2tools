using System;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    using CarNameConversion;

    public class CarIdArrayConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) =>
            text.Split(',').Select(carName => carName.ToCarID()).ToArray();

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) =>
            string.Join(",", ((Array)value).Cast<uint>().Select(carID => carID.ToCarName()));
    }
}