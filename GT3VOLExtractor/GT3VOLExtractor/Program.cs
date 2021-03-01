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
        private const uint HeaderMagic = 0xACB990AD; // RoFS, notted
        private static byte[] filenames;
        private static uint filenamesStart;

        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                if (args[0] == "-r")
                {
                    Import(args[1], args.Length > 2 ? args[2] : "gt3.vol");
                    return;
                }

                if (args[0] == "-e")
                {
                    Extract(args[1], args.Length > 2 ? args[2] : "extracted");
                    return;
                }
            }
            Console.WriteLine("Usage:\r\nExtract: GT3VOLExtractor -e <VOL file> [<Output directory>]\r\nRebuild: GT3VOLExtractor -r <Input directory> [<VOL file>]");
        }

        private static void Extract(string filename, string outputDirectory)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                if (file.ReadUInt() != HeaderMagic)
                {
                    Console.WriteLine("Not a valid VOL");
                    return;
                }
                ushort minorVersion = file.ReadUShort();
                ushort majorVersion = file.ReadUShort();
                if (minorVersion != 2 || majorVersion != 2)
                {
                    Console.WriteLine($"This tool only supports RoFS 2.2 VOL files. The provided VOL appears to be RoFS {majorVersion}.{minorVersion}");
                    return;
                }
                if (IsRoFS31VOL(file))
                {
                    Console.WriteLine($"This tool only supports RoFS 2.2 VOL files. The provided VOL appears to be RoFS 3.1. Please use GT4FS by team eventHorizon instead");
                    return;
                }
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

        private static bool IsRoFS31VOL(Stream file)
        {
            long currentFilePosition = file.Position;
            bool found31Header = false;
            var known31HeaderLocations = new uint[] { 0x1, 0x2159, 0x2231, 0x22B7 };
            foreach (uint headerLocation in known31HeaderLocations)
            {
                file.Position = headerLocation * FileEntry.BlockSize;
                if (file.ReadUInt() == HeaderMagic)
                {
                    found31Header = true;
                    break;
                }
            }
            file.Position = currentFilePosition;
            return found31Header;
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

        public static void Import(string path, string outputFilename)
        {
            var rootDirectory = new DirectoryEntry();
            rootDirectory.Import(path);
            rootDirectory.Name = ".";

            List<Entry> entries = BuildEntryList(rootDirectory);

            using (var output = new FileStream(outputFilename, FileMode.Create, FileAccess.ReadWrite))
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

                output.Position += FileEntry.BlockSize - (output.Position % FileEntry.BlockSize);

                foreach (var entry in entries)
                {
                    entry.Write(output);
                }
                output.Position -= 1;
                output.WriteByte(0);
            }
        }

        public static List<Entry> BuildEntryList(Entry rootDirectory)
        {
            var entries = new List<Entry> { rootDirectory };

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