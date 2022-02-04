using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class LicenseConverter : ITypeConverter
    {
        protected List<string> LicenseTypes = new List<string> { "None", "B", "A", "IC", "IB", "IA", "S" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)LicenseTypes.IndexOf(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte licenseType = (byte)value;
            return LicenseTypes[licenseType];
        }
    }
}