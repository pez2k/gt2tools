using System.Collections.Generic;
using System.IO;

namespace GT2.GT1ArchiveExtractor
{
    public class DirectoryFileList : FileList
    {
        private string path;

        public DirectoryFileList(string path) : base(path)
        {
            this.path = path;
        }

        public override IEnumerable<FileData> GetFiles()
        {
            IEnumerable<string> files = Directory.EnumerateFiles(path);

            foreach (string fileName in files)
            {
                byte[] contents = File.ReadAllBytes(fileName);
                var file = new FileData { Name = Path.GetFileNameWithoutExtension(fileName), Compressed = false, Contents = contents };
                if (file.IsArchive())
                {
                    yield return file;
                }
            }
        }
    }
}