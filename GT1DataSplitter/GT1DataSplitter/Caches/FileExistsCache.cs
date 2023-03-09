using System.IO;
using System.Collections.Generic;

namespace GT1.DataSplitter.Caches
{
    public static class FileExistsCache
    {
        private static readonly Dictionary<string, bool> cache = new();

        public static bool FileExists(string filename)
        {
            if (!cache.ContainsKey(filename))
            {
                bool exists = File.Exists(filename);
                cache.Add(filename, exists);
                return exists;
            }

            return cache[filename];
        }
    }
}