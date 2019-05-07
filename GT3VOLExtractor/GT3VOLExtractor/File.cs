using System;
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
    }
}