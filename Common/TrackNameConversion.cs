using System;

namespace GT2.TrackNameConversion
{
    public static class TrackNameConversion
    {
        public static string ToTrackName(this uint carID)
        {
            throw new NotImplementedException();
        }

        public static uint ToTrackID(this string carName)
        {
            uint trackID = 0;
            char[] carNameChars = carName.ToCharArray();

            foreach (char carNameChar in carNameChars)
            {
                byte asciiValue = (byte)carNameChar;
                var temp1 = trackID << 6;
                var temp2 = trackID >> 26;
                trackID = temp1 | temp2;
                trackID += asciiValue;
            }

            return trackID;
        }
    }
}
