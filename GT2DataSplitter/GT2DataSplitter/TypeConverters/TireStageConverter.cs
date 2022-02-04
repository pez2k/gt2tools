using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class TireStageConverter : ITypeConverter
    {
        protected List<string> TireTypes = new List<string> { "Stock", "Sports", "Hard", "Medium", "Soft", "SuperSoft", "Simulation", "Dirt" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)TireTypes.IndexOf(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte tireType = (byte)value;
            return TireTypes[tireType];
        }
    }
}