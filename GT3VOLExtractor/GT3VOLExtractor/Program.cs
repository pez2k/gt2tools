using System;
using System.IO;
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
            Extract(args[0]);
        }

        private static void Extract(string filename)
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
    }
}