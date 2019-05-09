using System;
using System.IO;

namespace GT3.VOLExtractor
{
    public abstract class Entry
    {
        protected long headerPosition;

        public string Name { get; set; }

        public static Entry Create(uint header) {
            header &= DirectoryEntry.Flag | ArchiveEntry.Flag;
            switch (header)
            {
                case 0:
                    return new FileEntry();
                case DirectoryEntry.Flag:
                    return new DirectoryEntry();
                case ArchiveEntry.Flag:
                    return new ArchiveEntry();
                default:
                    throw new Exception("Unknown VOL entry type");
            }
        }

        public abstract void Read(Stream stream);

        public abstract void Extract(string path, Stream stream);

        public abstract void Import(string path);

        public abstract void AllocateHeaderSpace(Stream stream);
    }
}