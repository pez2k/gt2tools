using CsvHelper.Configuration;

namespace GT3.DataSplitter
{
    public static class MemberMapExtensions
    {
        public static MemberMap<T, ushort> PartFilename<T>(this MemberMap<T, ushort> map, string partType)
        {
            return map.TypeConverter(Utils.GetFileNameConverter(partType));
        }
    }
}