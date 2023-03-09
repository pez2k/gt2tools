using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class LicenseConverter : ITypeConverter
    {
        private readonly List<string> licenseTypes = new() { "None", "B", "A", "IC", "IB", "IA", "S" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => (byte)licenseTypes.IndexOf(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => licenseTypes[(byte)value];
    }
}