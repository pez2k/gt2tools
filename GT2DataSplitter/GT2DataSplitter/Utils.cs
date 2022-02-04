namespace GT2.DataSplitter
{
    using TypeConverters;

    public static class Utils
    {
        public static CarIdConverter CarIdConverter { get; set; } = new CarIdConverter();
        public static CarIdArrayConverter CarIdArrayConverter { get; set; } = new CarIdArrayConverter();
        public static DrivetrainTypeConverter DrivetrainTypeConverter { get; set; } = new DrivetrainTypeConverter();
        public static TireWidthConverter TireWidthConverter { get; set; } = new TireWidthConverter();
        public static TireProfileConverter TireProfileConverter { get; set; } = new TireProfileConverter();
        public static TireStageConverter TireStageConverter { get; set; } = new TireStageConverter();
        public static LicenseConverter LicenseConverter { get; set; } = new LicenseConverter();
        public static DrivetrainRestrictionConverter DrivetrainRestrictionConverter { get; set; } = new DrivetrainRestrictionConverter();

        public static CachedFileNameConverter GetFileNameConverter(string name)
        {
            return new CachedFileNameConverter(name);
        }
    }
}