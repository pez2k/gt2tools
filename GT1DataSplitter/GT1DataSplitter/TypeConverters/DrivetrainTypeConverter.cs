using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class DrivetrainTypeConverter : ITypeConverter
    {
        private readonly List<string> drivetrainTypes = new() { "FR", "FF", "4WD", "MR", "RR" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => (byte)drivetrainTypes.IndexOf(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => drivetrainTypes[(byte)value];
    }
}