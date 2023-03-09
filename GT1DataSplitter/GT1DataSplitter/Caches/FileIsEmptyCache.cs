using System.IO;
using System.Collections.Generic;

namespace GT1.DataSplitter.Caches
{
    public static class FileIsEmptyCache
    {
        private static readonly Dictionary<string, bool> cache = new();

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