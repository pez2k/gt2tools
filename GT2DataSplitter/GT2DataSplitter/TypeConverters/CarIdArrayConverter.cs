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
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string[] inputs = text.Split(',');
            uint[] carIds = new uint[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                carIds[i] = inputs[i].ToCarID();
            }

            return carIds;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            uint[] carIds = ((Array)value).Cast<uint>().ToArray();
            string output = "";
            foreach (uint carId in carIds)
            {
                output += carId.ToCarName() + ",";
            }
            return output.TrimEnd(',');
        }
    }
}