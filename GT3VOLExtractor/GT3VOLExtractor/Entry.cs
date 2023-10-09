using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public abstract class Entry
    {
        public long HeaderPosition { get; set; }

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

        public abstract void Extract(string path, Stream stream, bool decompress);

        public abstract void Import(string path);

        public abstract void AllocateHeaderSpace(Stream stream);

        public void WriteFilename(Stream stream, Dictionary<string, uint> existingNames)
        {
            uint filenamePosition = (uint)stream.Position;
            stream.Position = HeaderPosition;
            if (existingNames.TryGetValue(Name, out uint existingPosition))
            {
                stream.WriteUInt(existingPosition | GetFlag());
                stream.Position = filenamePosition;
            }
            else
            {
                stream.WriteUInt(filenamePosition | GetFlag());
                stream.Position = filenamePosition;
                stream.Write(Encoding.ASCII.GetBytes(Name));
                stream.WriteByte(0);
                existingNames.Add(Name, filenamePosition);
            }
        }

        public virtual uint GetFlag() => 0;

        public abstract void Write(Stream stream);
    }
}