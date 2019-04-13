using System.Collections.Generic;

namespace GT1.ArchiveExtractor
{
    public abstract class FileList
    {
        public string Name;

        public FileList(string name)
        {
            Name = name;
        }

        public abstract IEnumerable<FileData> GetFiles();
    }
}