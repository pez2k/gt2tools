using System;
using System.IO;

namespace GT3.VOLExtractor
{
    public abstract class Entry
    {
        public string Name { get; set; }

        public static Entry Create(uint header) {
            header &= Directory.Flag | Archive.Flag;
            switch (header)
            {
                case 0:
                    return new File();
                case Directory.Flag:
                    return new Directory();
                case Archive.Flag:
                    return new Archive();
                default:
                    throw new Exception("Unknown VOL entry type");
            }
        }

        public abstract void Read(Stream stream);

        public abstract void Extract(string path, Stream stream);
    }
}