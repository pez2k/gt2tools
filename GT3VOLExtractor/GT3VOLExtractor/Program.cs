using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StreamExtensions;

namespace GT3.VOLExtractor
{
    class Program
    {
        private static byte[] filenames;
        private static uint filenamesStart;

        public static void Main(string[] args)
        {
            string outputDirectory = "extracted";
            if (args.Length > 1)
            {
                outputDirectory = args[1];
            }
            Extract(args[0], outputDirectory);
        }

        private static void Extract(string filename, string outputDirectory)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                if (file.ReadUInt() != 0xACB990AD) // RoFS, notted
                {
                    Console.WriteLine("Not a valid VOL");
                    return;
                }

                file.ReadUInt(); // unknown value, always 00 02 00 02
                uint headerSize = file.ReadUInt();
                filenamesStart = file.ReadUInt();
                uint fileCountMaybe = file.ReadUInt();
                LoadFilenames(file, headerSize);
                Entry rootDirectory = Entry.Create(file.ReadUInt());
                file.Position -= 4;
                rootDirectory.Read(file);
                rootDirectory.Extract(outputDirectory, file);
            }
        }

        private static void LoadFilenames(Stream file, uint filenamesEnd)
        {
            uint filenamesLength = filenamesEnd - filenamesStart;
            filenames = new byte[filenamesLength];
            long currentPosition = file.Position;
            file.Position = filenamesStart;
            file.Read(filenames);
            file.Position = currentPosition;
            for (int i = 0; i < filenames.Length; i++)
            {
                filenames[i] = (byte)~filenames[i];
            }
        }

        public static string GetFilename(uint position)
        {
            position -= filenamesStart;

            var filename = new StringBuilder();
            for (uint i = position; i < filenames.Length; i++)
            {
                byte character = filenames[i];
                if (character == 0)
                {
                    break;
                }
                filename.Append((char)character);
            }
            return $"{filename}";
        }

        public static void Import(string path)
        {
            var rootDirectory = new DirectoryEntry();
            rootDirectory.Import(path);
            rootDirectory.Name = ".";

            List<Entry> entries = BuildEntryList(rootDirectory);

            using (var output = new FileStream("new_gt3.vol", FileMode.Create, FileAccess.ReadWrite))
            {
                output.WriteUInt(0xACB990AD);
                output.WriteUInt(0x00020002);
                long headerSizePosition = output.Position;
                output.Position += 12;

                foreach (var entry in entries)
                {
                    entry.AllocateHeaderSpace(output);
                }

                //round up to nearest 8b, or just 4b gap?
                output.WriteUInt(0);

                uint stringTableStart = (uint)output.Position;
                output.WriteByte(0xFF);

                var newFilenames = new Dictionary<string, uint>();

                foreach (var entry in entries)
                {
                    entry.WriteFilename(output, newFilenames);
                }

                uint headerEnd = (uint)output.Position;
                output.Position = headerSizePosition;
                output.WriteUInt(headerEnd);
                output.WriteUInt(stringTableStart);
                uint filenamesLength = headerEnd - stringTableStart;
                output.WriteUInt(filenamesLength);

                output.Position = stringTableStart;
                byte[] filenameBytes = new byte[filenamesLength];
                output.Read(filenameBytes);
                for (int i = 0; i < filenamesLength; i++)
                {
                    filenameBytes[i] = (byte)~filenameBytes[i];
                }
                output.Position = stringTableStart;
                output.Write(filenameBytes);

                output.WriteByte(0);
            }
        }

        public static List<Entry> BuildEntryList(Entry rootDirectory)
        {
            var entries = new List<Entry>();
            entries.Add(rootDirectory);

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i] is DirectoryEntry directory)
                {
                    entries.AddRange(directory.Entries.Where(entry => entry.Name != ".."));
                }
            }

            return entries;
        }
    }
}