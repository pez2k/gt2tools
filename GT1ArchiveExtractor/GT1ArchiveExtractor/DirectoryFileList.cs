using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                else
                {
                    var headerTest = new FileData { Contents = file.Contents.Skip(1).Take(8).Concat(file.Contents.Skip(10).Take(2)).ToArray() };

                    if (headerTest.IsArchive())
                    {
                        file.Compressed = true;
                        yield return file;
                    }
                }
            }
        }
    }
}