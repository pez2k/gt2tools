namespace GT2.DataSplitter.GTDT.ArcadeData
{
    public static class ArcadeStrings
    {
        // Arcade doesn't have a corresponding unistrdb, so hardcode what it seems to need for the Engine parts
        private static readonly string[] strings =
        [
            "Rotar2",
            "Rotary",
            "TURBO",
            "5000rpm",
            "I4",
            "DOHC",
            "4500rpm",
            "V12",
            "-",
            "NA",
            "3450rpm",
            "V8",
            "MA",
            "3600rpm",
            "OHV",
            "3000rpm",
            "3900rpm",
            "Boxer6",
            "SOHC",
            "V6",
            "4000rpm",
            "V10",
            "3700rpm",
            "5200rpm",
            "4250rpm",
            "3500rpm",
            "5500rpm",
            "6000rpm",
            "2200-5500rpm",
            "I6",
            "1750-4600rpm",
            "3800rpm",
            "7500rpm",
            "I5",
            "2500rpm",
            "4900rpm",
            "6400rpm",
            "4400rpm",
            "4800rpm",
            "I3",
            "Boxer4",
            "4750rpm",
            "5250rpm"
        ];

        public static UnicodeStringTable GetStringTable()
        {
            UnicodeStringTable table = new();
            table.AddRange(strings);
            return table;
        }
    }
}