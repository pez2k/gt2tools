using System.IO;

namespace GT2.GT1ArchiveExtractor
{
    using StreamExtensions;

    public class DiskFileWriter : IFileWriter
    {
        public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void Write(string path, byte[] contents)
        {
            using (FileStream output = File.OpenWrite(path))
            {
                output.Write(contents);
            }
        }
    }
}