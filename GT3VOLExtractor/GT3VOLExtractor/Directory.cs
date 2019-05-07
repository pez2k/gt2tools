using System.Collections.Generic;
using System.IO;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public class Directory : Entry
    {
        public const uint Flag = 0x01000000;

        public List<Entry> Entries { get; set; }

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
                    Entries.Add(new Directory { Name = ".." });
                    continue;
                }

                stream.Position = entryPosition;
                Entries.Add(Create(stream.ReadUInt()));
                stream.Position -= 4;
                Entries[i].Read(stream);
                stream.Position = currentPosition;
            }
        }
    }
}