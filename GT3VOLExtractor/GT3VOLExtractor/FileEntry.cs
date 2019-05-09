using System;
using System.IO;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    public class FileEntry : Entry
    {
        public uint Location { get; set; }
        public uint Size { get; set; }

        public override void Read(Stream stream)
        {
            Console.WriteLine($"{stream.Position}");
            uint filenamePosition = stream.ReadUInt();
            Name = Program.GetFilename(filenamePosition);
            Location = stream.ReadUInt();
            Size = stream.ReadUInt();
        }

        public override void Extract(string path, Stream stream)
        {
            path = Path.Combine(path, Name);
            Console.WriteLine($"Extracting file: {path}");
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
            Console.WriteLine(path);
            Name = Path.GetFileName(path);
        }

        public override void AllocateHeaderSpace(Stream stream)
        {
            headerPosition = stream.Position;
            Console.WriteLine($"{headerPosition}");
            stream.Position += 12;
        }
    }
}