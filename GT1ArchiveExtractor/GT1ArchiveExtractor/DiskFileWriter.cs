using System.Collections.Generic;
using System.IO;

namespace GT2.GT1ArchiveExtractor
{
    using StreamExtensions;

    public class DiskFileWriter : IFileWriter
    {
        private Dictionary<string, string> fileNames;

        public DiskFileWriter(string fileNameDataPath = null)
        {
            fileNames = new Dictionary<string, string>();

            if (fileNameDataPath != null)
            {
                foreach (string name in File.ReadAllLines(fileNameDataPath))
                {
                    string[] parts = name.Split(' ');
                    fileNames.Add(parts[0], parts[1]);
                }
            }
        }

        public void CreateDirectory(string path)
        {
            path = CheckForName(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void Write(string path, byte[] contents)
        {
            path = CheckForName(path);

            using (FileStream output = File.OpenWrite(path))
            {
                output.Write(contents);
            }
        }

        private string CheckForName(string path)
        {
            if (fileNames.ContainsKey(path))
            {
                fileNames.TryGetValue(path, out path);
            }
            return path;
        }
    }
}