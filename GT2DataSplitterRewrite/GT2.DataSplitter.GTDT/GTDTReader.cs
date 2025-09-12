namespace GT2.DataSplitter.GTDT
{
    using ArcadeData;
    using LicenseData;
    using Models;

    public static class GTDTReader
    {
        public static GTModeModel ReadGTMode(string languagePrefix)
        {
            if (languagePrefix != "")
            {
                languagePrefix += "_";
            }

            UnicodeStringTable strings = new();
            strings.Read($"{languagePrefix}unistrdb.dat.gz");
            GTModeDataFile data = new();
            data.Read($"{languagePrefix}gtmode_data.dat.gz");
            GTModeRaceFile race = new();
            race.Read($"{languagePrefix}gtmode_race.dat.gz");

            GTModeModel model = new();
            data.MapToModel(model, strings);
            race.MapToModel(model, strings);

            return model;
        }

        public static ArcadeModel ReadArcade(string languagePrefix)
        {
            if (languagePrefix != "")
            {
                languagePrefix += "_";
            }

            UnicodeStringTable strings = ArcadeStrings.GetStringTable();
            ArcadeDataFile data = new();
            data.Read($"{languagePrefix}arcade_data.dat.gz");

            ArcadeModel model = new();
            data.MapToModel(model, strings);

            return model;
        }

        public static LicenseModel ReadLicense(string languagePrefix)
        {
            if (languagePrefix != "")
            {
                languagePrefix += "_";
            }

            UnicodeStringTable strings = LicenseStrings.GetStringTable();
            LicenseDataFile data = new();
            data.Read($"{languagePrefix}license_data.dat.gz");

            LicenseModel model = new();
            data.MapToModel(model, strings);

            return model;
        }
    }
}