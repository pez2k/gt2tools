using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class TireStageConverter : ITypeConverter
    {
        private readonly List<string> tireTypes = new() { "Stock", "Sports", "Hard", "Medium", "Soft", "SuperSoft", "Simulation", "Dirt" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => (byte)tireTypes.IndexOf(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => tireTypes[(byte)value];
    }
}