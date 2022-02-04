using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class DrivetrainRestrictionConverter : ITypeConverter
    {
        protected List<string> DrivetrainTypes = new List<string> { "None", "FF", "FR", "MR", "RR", "4WD" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)DrivetrainTypes.IndexOf(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte drivetrainType = (byte)value;
            return DrivetrainTypes[drivetrainType];
        }
    }
}