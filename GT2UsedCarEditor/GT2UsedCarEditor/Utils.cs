using System.Collections.Generic;

namespace GT2UsedCarEditor
{
    public static class Utils
    {
        private static List<char> characterSet =
            new List<char> { '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public static string GetCarName(uint carID)
        {
            string carName = "";

            for (int i = 0; i < 5; i++)
            {
                uint currentCharNo = (carID >> (i * 6)) & 0x3F;
                carName = characterSet[(int)currentCharNo] + carName;
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
                currentCarID += characterSet.IndexOf(carNameChar);
                currentCarID = currentCarID << 6;
            }
            currentCarID = currentCarID >> 6;
            carID = (uint)currentCarID;
            return carID;
        }
    }
}
