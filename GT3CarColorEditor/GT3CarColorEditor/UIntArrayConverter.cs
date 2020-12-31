using System;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT3.CarColorEditor
{
    public class UIntArrayConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string[] inputs = text.Split(',');
            uint[] arrayData = new uint[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                arrayData[i] = uint.Parse(inputs[i]);
            }

            return arrayData;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            uint[] arrayData = ((Array)value).Cast<uint>().ToArray();
            string output = "";
            foreach (uint carId in arrayData)
            {
                output += $"{carId},";
            }
            return output.TrimEnd(',');
        }
    }
}