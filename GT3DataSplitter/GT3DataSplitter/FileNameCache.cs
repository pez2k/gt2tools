using System;
using System.Collections.Generic;

namespace GT3.DataSplitter
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

        public static string Get(string type, int index)
        {
            if (!Cache.ContainsKey(type) || Cache[type].Count < index)
            {
                return $"{index}";
            }

            return Cache[type][index];
        }

        public static int Get(string type, string filename)
        {
            if (!Cache.ContainsKey(type) || !Cache[type].Contains(filename))
            {
                if (int.TryParse(filename, out int rawData))
                {
                    return rawData;
                }
                throw new Exception($"Filename {filename} of type {type} not found.");
            }

            return Cache[type].IndexOf(filename);
        }
    }
}
