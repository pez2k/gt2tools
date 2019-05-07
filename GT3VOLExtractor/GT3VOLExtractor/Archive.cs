using System.IO;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public class Archive : File
    {
        public const uint Flag = 0x02000000;

        public uint UncompressedSize { get; set; }

        public override void Read(Stream stream)
        {
            uint filenamePosition = stream.ReadUInt() - Flag;
            Name = Program.GetFilename(filenamePosition);
            Location = stream.ReadUInt();
            UncompressedSize = stream.ReadUInt();
            Size = stream.ReadUInt();
        }

        public override void Extract(string path, Stream stream)
        {
            Name += ".gz";
            base.Extract(path, stream);
        }
    }
}