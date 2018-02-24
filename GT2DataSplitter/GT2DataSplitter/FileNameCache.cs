using System.Collections.Generic;

namespace GT2DataSplitter
{
    public static class FileNameCache
    {
        public static Dictionary<string, List<string>> Cache { get; set; } = new Dictionary<string, List<string>>();

        public static void Add(string type, string filename)
        {
            if (!Cache.ContainsKey(type))
            {
                Cache.Add(type, new List<string>());
            }
            Cache[type].Add(filename);
        }

        public static string Get(string type, ushort index)
        {
            if (!Cache.ContainsKey(type) || Cache[type].Count < index)
            {
                return "";
            }

            return Cache[type][index];
        }

        public static ushort Get(string type, string filename)
        {
            if (!Cache.ContainsKey(type) || !Cache[type].Contains(filename))
            {
                return 0;
            }

            return (ushort)Cache[type].IndexOf(filename);
        }
    }
}
