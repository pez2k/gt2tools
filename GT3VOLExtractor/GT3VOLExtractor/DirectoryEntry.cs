using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public class DirectoryEntry : Entry
    {
        public const uint Flag = 0x01000000;

        public List<Entry> Entries { get; set; }

        public DirectoryEntry ParentDirectory { get; set; }

        public override void Read(Stream stream)
        {
            uint filenamePosition = stream.ReadUInt() - Flag;
            Name = Program.GetFilename(filenamePosition);
            uint numberOfEntries = stream.ReadUInt();
            Entries = new List<Entry>((int)numberOfEntries);

            for (int i = 0; i < numberOfEntries; i++)
            {
                uint entryPosition = stream.ReadUInt();
                long currentPosition = stream.Position;
                if (entryPosition < currentPosition)
                {
                    Entries.Add(new DirectoryEntry { Name = ".." });
                    continue;
                }

                stream.Position = entryPosition;
                Entries.Add(Create(stream.ReadUInt()));
                stream.Position -= 4;
                Entries[i].Read(stream);
                stream.Position = currentPosition;
            }
        }

        public override void Extract(string path, Stream stream, bool decompress)
        {
            path = Path.Combine(path, Name);
            Console.WriteLine($"Extracting directory: {path}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var entry in Entries)
            {
                if (entry.Name != "..")
                {
                    entry.Extract(path, stream, decompress);
                }
            }
        }

        public override void Import(string path)
        {
            Name = Path.GetFileName(path);
            List<string> childPaths = Directory.EnumerateFileSystemEntries(path).ToList();
            childPaths.Sort(StringComparer.Ordinal);

            Entries = new List<Entry> { new DirectoryEntry { Name = ".." } };

            foreach (string childPath in childPaths)
            {
                Entry entry = (File.GetAttributes(childPath) & FileAttributes.Directory) != 0
                                  ? new DirectoryEntry { ParentDirectory = this }
                                  : (Entry)(ArchiveEntry.FileExtensions.Contains(Path.GetExtension(childPath)) ? new ArchiveEntry()
                                                                                                               : new FileEntry());
                entry.Import(childPath);
                Entries.Add(entry);
            }
        }

        public override void AllocateHeaderSpace(Stream stream)
        {
            HeaderPosition = stream.Position;
            stream.Position += 8;
            stream.Position += 4 * Entries.Count;
            Entries[0].HeaderPosition = ParentDirectory?.HeaderPosition ?? 0;
        }

        public override uint GetFlag() => Flag;

        public override void Write(Stream stream)
        {
            Console.WriteLine($"Importing directory: {Name}");
            uint currentPosition = (uint)stream.Position;
            stream.Position = HeaderPosition + 4;
            stream.WriteUInt((uint)Entries.Count);
            foreach (var entry in Entries)
            {
                stream.WriteUInt((uint)entry.HeaderPosition);
            }
            stream.Position = currentPosition;
        }
    }
}