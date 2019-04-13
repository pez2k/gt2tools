using System.Collections.Generic;
using System.IO;

namespace GT1.ArchiveExtractor
{
    using GT2.StreamExtensions;

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
            string filename = Path.GetFileName(path);
            string directory = null;
            if (filename.Length > 0)
            {
                directory = path.Replace($"\\{filename}", "");
            }

            if (fileNames.ContainsKey(path))
            {
                fileNames.TryGetValue(path, out path);
            }
            else if (directory != null && fileNames.ContainsKey(directory))
            {
                string newDirectory;
                fileNames.TryGetValue(directory, out newDirectory);
                path = path.Replace(directory, newDirectory);
            }
            return path;
        }
    }
}