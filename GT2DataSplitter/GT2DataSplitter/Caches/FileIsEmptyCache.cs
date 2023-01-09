using System.IO;
using System.Collections.Generic;

namespace GT2.DataSplitter.Caches
{
    public static class FileIsEmptyCache
    {
        private static readonly Dictionary<string, bool> cache = new Dictionary<string, bool>();

        public static bool FileIsEmpty(string filename)
        {
            if (!cache.ContainsKey(filename))
            {
                bool isEmpty = new FileInfo(filename).Length == 0;
                cache.Add(filename, isEmpty);
                return isEmpty;
            }

            return cache[filename];
        }
    }
}