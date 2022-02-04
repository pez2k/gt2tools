using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using TypeConverters;

    public static class MemberMapExtensions
    {
        public static MemberMap<TStructure, TDataType> CarId<TStructure, TDataType>(this MemberMap<TStructure, TDataType> map) =>
            map.TypeConverter(new CarIdConverter());

        public static MemberMap<TStructure, TDataType> PartFilename<TStructure, TDataType>(this MemberMap<TStructure, TDataType> map, string partType) =>
            map.TypeConverter(new CachedFileNameConverter(partType));
    }
}