using System;
using System.IO;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public class FileEntry : Entry
    {
        public uint Location { get; set; }
        public uint Size { get; set; }

        protected string filePath;

        public override void Read(Stream stream)
        {
            //Console.WriteLine($"{stream.Position}");
            uint filenamePosition = stream.ReadUInt();
            Name = Program.GetFilename(filenamePosition);
            Location = stream.ReadUInt();
            Size = stream.ReadUInt();
        }

        public override void Extract(string path, Stream stream)
        {
            path = Path.Combine(path, Name);
            Console.WriteLine($"Extracting file: {path}");
            //Console.WriteLine($"{Location},{Size},{path}");
            stream.Position = Location * 0x800;
            using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[Size];
                stream.Read(buffer);
                output.Write(buffer);
            }
        }

        public override void Import(string path)
        {
            //Console.WriteLine(path);
            filePath = path;
            Name = Path.GetFileName(path);
        }

        public override void AllocateHeaderSpace(Stream stream)
        {
            HeaderPosition = stream.Position;
            //Console.WriteLine($"{HeaderPosition}");
            stream.Position += 12;
        }

        public override void Write(Stream stream)
        {
            Console.WriteLine($"Importing file: {filePath}");
            uint filePosition = (uint)stream.Position;
            stream.Position = HeaderPosition + 4;
            stream.WriteUInt(filePosition / 0x800);

            using (var input = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.WriteUInt((uint)input.Length);
                stream.Position = filePosition;
                input.CopyTo(stream);
            }

            stream.Position += 0x800 - (stream.Position % 0x800);
        }
    }
}