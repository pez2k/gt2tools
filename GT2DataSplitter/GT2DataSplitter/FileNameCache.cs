using System.Collections.Generic;

namespace GT2DataSplitter
{
    public static class FileNameCache
    {
        public static Dictionary<string, Dictionary<ushort, string>> Cache { get; set; } = new Dictionary<string, Dictionary<ushort, string>>();

        public static ushort Count { get; set; } = 0;

        public static void Add(string type, string filename)
        {
            Add(type, Count, filename);
        }

        public static void Add(string type, ushort index, string filename)
        {
            if (!Cache.ContainsKey(type))
            {
                Cache.Add(type, new Dictionary<ushort, string>());
            }
            Cache[type].Add(index, filename);
            Count++;
        }

        public static string Get(string type, ushort index)
        {
            if (!Cache.ContainsKey(type) || !Cache[type].ContainsKey(index))
            {
                return "";
            }

            return Cache[type][index];
        }
    }
}
