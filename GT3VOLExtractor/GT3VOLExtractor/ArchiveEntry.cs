using System;
using System.IO;
using System.Linq;
using StreamExtensions;
using System.IO.Compression;

namespace GT3.VOLExtractor
{
    public class ArchiveEntry : FileEntry
    {
        public const uint Flag = 0x02000000;
        private const string GZipExtension = ".gz";
        private const string PS2ZipExtension = ".ps2zip";
        private const string UnknownCompressionExtension = ".compressed";
        public static string[] FileExtensions = { GZipExtension, PS2ZipExtension, UnknownCompressionExtension };

        public uint UncompressedSize { get; set; }

        public override void Read(Stream stream)
        {
            uint filenamePosition = stream.ReadUInt() - Flag;
            Name = Program.GetFilename(filenamePosition);
            Location = stream.ReadUInt();
            UncompressedSize = stream.ReadUInt();
            Size = stream.ReadUInt();
        }

        public override void Extract(string path, Stream stream, bool decompress)
        {
            var magic = new byte[4];
            stream.Position = Location * BlockSize;
            stream.Read(magic);

            if (decompress)
            {
                path = Path.Combine(path, Name);
                stream.Position = Location * BlockSize;

                if (GetExtension(magic) == PS2ZipExtension)
                {
                    Console.WriteLine($"Decompressing PS2ZIP (inflate) file: {path}");
                    using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        int magic_ = stream.ReadInt();
                        int decompSize = -stream.ReadInt();

                        var deflateStream = new DeflateStream(stream, CompressionMode.Decompress);

                        byte[] buffer = new byte[UncompressedSize];
                        deflateStream.Read(buffer);
                        output.Write(buffer);
                    }
                }
                else if (GetExtension(magic) == GZipExtension)
                {
                    Console.WriteLine($"Decompressing GZip file: {path}");
                    using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        var deflateStream = new GZipStream(stream, CompressionMode.Decompress);

                        byte[] buffer = new byte[UncompressedSize];
                        deflateStream.Read(buffer);
                        output.Write(buffer);
                    }
                }
                else
                {
                    Console.WriteLine($"Extracting file: {path}");
                    base.Extract(path, stream, decompress);
                }
            }
            else
            {
                Name += GetExtension(magic);
                Console.WriteLine($"Extracting file: {path}");
                base.Extract(path, stream, decompress);
            }
        }

        private string GetExtension(byte[] magic)
        {
            if (magic[0] == 0x1F && magic[1] == 0x8B)
            {
                return GZipExtension;
            }
            else if (magic.SequenceEqual(new byte[] { 0xC5, 0xEE, 0xF7, 0xFF }))
            {
                return PS2ZipExtension;
            }
            return UnknownCompressionExtension;
        }

        public override void Import(string path)
        {
            base.Import(path);
            if (Name.EndsWith(GZipExtension))
            {
                Name = Name.Substring(0, Name.Length - GZipExtension.Length);
            }
            else if (Name.EndsWith(PS2ZipExtension))
            {
                Name = Name.Substring(0, Name.Length - PS2ZipExtension.Length);
            }
            else if (Name.EndsWith(UnknownCompressionExtension))
            {
                Name = Name.Substring(0, Name.Length - UnknownCompressionExtension.Length);
            }
        }

        public override void AllocateHeaderSpace(Stream stream)
        {
            HeaderPosition = stream.Position;
            stream.Position += 16;
        }

        public override uint GetFlag() => Flag;

        public override void Write(Stream stream)
        {
            Console.WriteLine($"Importing file: {filePath}");
            uint filePosition = (uint)stream.Position;
            stream.Position = HeaderPosition + 4;
            stream.WriteUInt(filePosition / BlockSize);

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

            stream.Position += BlockSize - (stream.Position % BlockSize);
        }
    }
}