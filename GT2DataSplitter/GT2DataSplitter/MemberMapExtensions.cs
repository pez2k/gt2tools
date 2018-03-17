using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    public static class MemberMapExtensions
    {
        public static MemberMap<T, ushort> PartFilename<T>(this MemberMap<T, ushort> map, string partType)
        {
            return map.TypeConverter(Utils.GetFileNameConverter(partType));
        }
    }
}