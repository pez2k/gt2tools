using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GT2DataSplitter
{
    public static class Utils
    {
        public static CarIdConverter CarIdConverter { get; set; } = new CarIdConverter();

        private static char[] characterSet = { '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static List<char> characterList = characterSet.ToList();

        public static string GetCarName(uint carID)
        {
            string carName = "";

            for (int i = 0; i < 5; i++)
            {
                uint currentCharNo = (carID >> (i * 6)) & 0x3F;
                carName = characterSet[currentCharNo] + carName;
            }

            return carName;
        }

        public static uint GetCarID(string carName)
        {
            uint carID = 0;
            char[] carNameChars = carName.ToCharArray();
            long currentCarID = 0;

            foreach (char carNameChar in carNameChars)
            {
                currentCarID += characterList.IndexOf(carNameChar);
                currentCarID = currentCarID << 6;
            }
            currentCarID = currentCarID >> 6;
            carID = (uint)currentCarID;
            return carID;
        }
    }

    public class CarIdConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            uint carId = (uint)value;
            return Utils.GetCarName(carId);
        }
    }
}
