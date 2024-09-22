using System;

namespace GT2.SaveEditor.GTMode.Garage
{
    public static class WheelFilenameConverter
    {
        private static readonly string[] wheelManufacturers = new[]
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

        private static readonly string[] wheelLugs = new[]
        {
            "-",
            "4",
            "5",
            "6"
        };

        public static uint ConvertFromString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0x100;
            }
            if (text.Length != 8)
            {
                throw new Exception($"Wheel ID must be 8 characters long: {text}");
            }
            string manufacturer = text[..2];
            uint manufacturerIDPart;
            for (manufacturerIDPart = 0; manufacturerIDPart < wheelManufacturers.Length; manufacturerIDPart++)
            {
                if (wheelManufacturers[manufacturerIDPart] == manufacturer)
                {
                    break;
                }
            }
            manufacturerIDPart = manufacturerIDPart * 0x10 << 24;
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
            lugsPart = lugsPart * 0x20 << 8;
            uint colourPart = text.Substring(7, 1)[0];
            return manufacturerIDPart + wheelNumberPart + lugsPart + colourPart + 0x200;
        }

        public static string ConvertToString(uint data)
        {
            if (data == 0)
            {
                return "";
            }

            string manufacturer = wheelManufacturers[(data >> 24) / 0x10];
            uint wheelNumber = data >> 16 & 0xFF;
            string lugs = wheelLugs[(data >> 8 & 0xFF) / 0x20];
            char colour = (char)(data & 0xFF);
            return $"{manufacturer}{wheelNumber:D3}-{lugs}{colour}";
        }
    }
}