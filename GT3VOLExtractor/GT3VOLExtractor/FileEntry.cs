using System;
using System.IO;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public class FileEntry : Entry
    {
        public const uint BlockSize = 0x800;

        public uint Location { get; set; }
        public uint Size { get; set; }

        protected string filePath;

        public override void Read(Stream stream)
        {
            uint filenamePosition = stream.ReadUInt();
            Name = Program.GetFilename(filenamePosition);
            Location = stream.ReadUInt();
            Size = stream.ReadUInt();
        }

        public override void Extract(string path, Stream stream, bool decompress)
        {
            path = Path.Combine(path, Name);
            Console.WriteLine($"Extracting file: {path}");
            stream.Position = Location * BlockSize;
            using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[Size];
                stream.Read(buffer);
                output.Write(buffer);
            }
        }

        public override void Import(string path)
        {
            filePath = path;
            Name = Path.GetFileName(path);
        }

        public override void AllocateHeaderSpace(Stream stream)
        {
            HeaderPosition = stream.Position;
            stream.Position += 12;
        }

        public override void Write(Stream stream)
        {
            Console.WriteLine($"Importing file: {filePath}");
            uint filePosition = (uint)stream.Position;
            stream.Position = HeaderPosition + 4;
            stream.WriteUInt(filePosition / BlockSize);

            using (var input = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.WriteUInt((uint)input.Length);
                stream.Position = filePosition;
                input.CopyTo(stream);
            }

            stream.Position += BlockSize - (stream.Position % BlockSize);
        }
    }
}