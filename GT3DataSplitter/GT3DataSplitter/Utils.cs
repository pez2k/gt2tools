using System;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT3.DataSplitter
{
    public static class Utils
    {
        public static IdConverter IdConverter { get; set; } = new IdConverter();
        public static IdArrayConverter IdArrayConverter { get; set; } = new IdArrayConverter();
    }

    public class IdConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => Program.IDStrings.Add(text);

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) => Program.IDStrings.Get((ulong)value) ?? "";
    }

    public class IdArrayConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string[] inputs = text.Split(',');
            ulong[] hashes = new ulong[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                hashes[i] = Program.IDStrings.Add(inputs[i].Trim());
            }

            return hashes;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            ulong[] hashes = ((Array)value).Cast<ulong>().ToArray();
            string output = "";
            foreach (ulong hash in hashes)
            {
                output += Program.IDStrings.Get(hash) + ", ";
            }
            return output.TrimEnd(',', ' ');
        }
    }
}
