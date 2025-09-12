namespace GT2.DataSplitter.GTDT.LicenseData
{
    public static class LicenseStrings
    {
        // License doesn't have a corresponding unistrdb, so hardcode what it seems to need for the Engine parts
        private static readonly string[] strings =
        [
            "Rotar2",
            "Rotary",
            "TURBO",
            "5000rpm",
            "I4",
            "SOHC",
            "NA",
            "4500rpm",
            "DOHC",
            "V6",
            "OHV",
            "3000rpm",
            "V8",
            "2400rpm",
            "V10",
            "3700rpm",
            "4250rpm",
            "4000rpm",
            "3500rpm",
            "4200rpm",
            "5500rpm",
            "6200rpm",
            "7500rpm",
            "5300rpm",
            "9500rpm",
            "I5",
            "2500rpm",
            "6000rpm",
            "4400rpm",
            "I6",
            "4800rpm",
            "Boxer4",
            "6400rpm",
            "6800rpm",
            "3600rpm",
            "-",
            "6500rpm"
        ];

        public static UnicodeStringTable GetStringTable()
        {
            UnicodeStringTable table = new();
            table.AddRange(strings);
            return table;
        }
    }
}