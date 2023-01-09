using System.IO;
using System.Collections.Generic;

namespace GT2.DataSplitter.Caches
{
    public static class FileContentsCache
    {
        private static readonly Dictionary<string, byte[]> cache = new Dictionary<string, byte[]>();

        public static byte[] GetFile(string filename)
        {
            if (!cache.ContainsKey(filename))
            {
                using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    var data = new byte[file.Length];
                    file.Read(data, 0, (int)file.Length);
                    cache.Add(filename, data);
                    return data;
                }
            }

            return cache[filename];
        }
    }
}