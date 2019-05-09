using System.IO;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public class ArchiveEntry : FileEntry
    {
        public const uint Flag = 0x02000000;

        public uint UncompressedSize { get; set; }

        public override void Read(Stream stream)
        {
            System.Console.WriteLine($"{stream.Position}");
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

        public override void Import(string path)
        {
            base.Import(path);
            Name = Name.Replace(".gz", "");
        }

        public override void AllocateHeaderSpace(Stream stream)
        {
            HeaderPosition = stream.Position;
            System.Console.WriteLine($"{HeaderPosition}");
            stream.Position += 16;
        }

        public override uint GetFlag() => Flag;
    }
}