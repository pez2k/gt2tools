using System.IO;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public class File : Entry
    {
        public uint Location { get; set; }
        public uint Size { get; set; }

        public override void Read(Stream stream)
        {
            uint filenamePosition = stream.ReadUInt();
            Name = Program.GetFilename(filenamePosition);
            Location = stream.ReadUInt();
            Size = stream.ReadUInt();
        }
    }
}