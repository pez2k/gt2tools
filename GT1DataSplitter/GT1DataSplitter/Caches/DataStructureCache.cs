using System.Collections.Generic;

namespace GT1.DataSplitter.Caches
{
    public static class DataStructureCache
    {
        private static readonly Dictionary<string, DataStructure> cache = new();

        public static void Add(string filename, DataStructure structure) => cache.Add(filename, structure);

        public static DataStructure GetStructure(string filename) => cache.ContainsKey(filename) ? cache[filename] : null;
    }
}