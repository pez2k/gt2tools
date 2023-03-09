using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT1.DataSplitter.TypeConverters
{
    public class WheelIdConverter : ITypeConverter
    {
        private readonly string[] wheelManufacturers = new []
        {
            "bb",
            "br",
            "du",
            "en",
            "fa",
            "oz",
            "ra",
            "sp",
            "yo"
        };

        private readonly string[] wheelLugs = new []
        {
            "-",
            "4",
            "5",
            "6"
        };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return (uint)0;
            }
            if (text.Length != 8)
            {
                throw new Exception($"Wheel ID must be 8 characters long: {text}");
            }
            string manufacturer = text.Substring(0, 2);
            uint manufacturerIDPart;
            for (manufacturerIDPart = 0; manufacturerIDPart < wheelManufacturers.Length; manufacturerIDPart++)
            {
                if (wheelManufacturers[manufacturerIDPart] == manufacturer)
                {
                    break;
                }
            }
            manufacturerIDPart = (manufacturerIDPart * 0x10) << 24;
            uint wheelNumberPart = uint.Parse(text.Substring(2, 3)) << 16;
            uint lugsPart;
            string lugs = text.Substring(6, 1);
            for (lugsPart = 0; lugsPart < wheelLugs.Length; lugsPart++)
            {
                if (wheelLugs[lugsPart] == lugs)
                {
                    break;
                }
            }
            lugsPart = (lugsPart * 0x20) << 8;
            uint colourPart = text.Substring(7, 1)[0];
            return manufacturerIDPart + wheelNumberPart + lugsPart + colourPart;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            uint data = (uint)value;
            if (data == 0)
            {
                return "";
            }

            string manufacturer = wheelManufacturers[(data >> 24) / 0x10];
            uint wheelNumber = (data >> 16) & 0xFF;
            string lugs = wheelLugs[((data >> 8) & 0xFF) / 0x20];
            char colour = (char)(data & 0xFF);
            return $"{manufacturer}{wheelNumber:D3}-{lugs}{colour}";
        }
    }
}