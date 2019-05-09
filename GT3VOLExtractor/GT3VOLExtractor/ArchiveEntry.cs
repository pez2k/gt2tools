using System;
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
            //Console.WriteLine($"{stream.Position}");
            uint filenamePosition = stream.ReadUInt() - Flag;
            Name = Program.GetFilename(filenamePosition);
            Location = stream.ReadUInt();
            UncompressedSize = stream.ReadUInt();
            Size = stream.ReadUInt();
            //Console.WriteLine($"{Location},{Size},{Name},Archive");
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
            //Console.WriteLine($"{HeaderPosition}");
            stream.Position += 16;
        }

        public override uint GetFlag() => Flag;

        public override void Write(Stream stream)
        {
            Console.WriteLine($"Importing file: {filePath}");
            uint filePosition = (uint)stream.Position;
            stream.Position = HeaderPosition + 4;
            stream.WriteUInt(filePosition / 0x800);

            using (var input = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                input.Position = input.Length - 4;
                uint uncompressedSize = input.ReadUInt();
                input.Position = 0;
                stream.WriteUInt(uncompressedSize);
                stream.WriteUInt((uint)input.Length);
                stream.Position = filePosition;
                input.CopyTo(stream);
            }

            stream.Position += 0x800 - (stream.Position % 0x800);
        }
    }
}