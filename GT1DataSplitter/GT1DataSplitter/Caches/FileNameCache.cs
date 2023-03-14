using System;
using System.Collections.Generic;

namespace GT1.DataSplitter.Caches
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

        public static string Get(string type, int index) =>
            index == -1 ? "None" : Cache[type][index];

        public static int Get(string type, string filename)
        {
            return !Cache.ContainsKey(type) || !Cache[type].Contains(filename)
                ? filename == "None" ? -1 : throw new Exception($"Filename {filename} of type {type} not found.")
                : Cache[type].IndexOf(filename);
        }
    }
}
