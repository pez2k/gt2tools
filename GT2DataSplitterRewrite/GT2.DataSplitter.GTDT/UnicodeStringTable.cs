using System.IO.Compression;
using System.Text;

namespace GT2.DataSplitter.GTDT
{
    using StreamExtensions;

    public class UnicodeStringTable
    {
        private readonly List<string> strings = new();
        private static readonly Encoding binaryEncoding = Encoding.Unicode;

        public void Read(string filename)
        {
            using (Stream file = DecompressFile(filename))
            {
                file.Position = 8;
                ushort stringCount = file.ReadUShort();

                for (ushort i = 0; i < stringCount; i++)
                {
                    int length = (file.ReadUShort() + 1) * 2;
                    byte[] characterData = new byte[length];
                    file.Read(characterData, 0, length);

                    strings.Add(binaryEncoding.GetString(characterData).TrimEnd('\0'));
                }
            }
        }

        public static Stream DecompressFile(string filename)
        {
            MemoryStream stream = new();
            using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
            {
                if (filename.EndsWith(".gz"))
                {
                    using (GZipStream unzip = new(file, CompressionMode.Decompress))
                    {
                        unzip.CopyTo(stream);
                    }
                }
                else
                {
                    file.CopyTo(stream);
                }
            }
            return stream;
        }

        public void Write(string filename)
        {
            using (FileStream file = new(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                file.WriteUInt(0);
                file.WriteCharacters("WSDB");
                file.WriteUShort((ushort)strings.Count);

                foreach (string newString in strings)
                {
                    byte[] characters = binaryEncoding.GetBytes((newString + "\0").ToCharArray());
                    ushort length = (ushort)((characters.Length - 1) / 2);
                    file.WriteUShort(length);
                    file.Write(characters, 0, characters.Length);
                }

                file.Position = 0;
                file.WriteUInt((uint)file.Length);

                if (file.Length > 0x6000)
                {
                    throw new Exception("unistrdb.dat exceeds 24kb size limit.");
                }

                file.Position = 0;
                using (FileStream zipFile = new(filename + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new(zipFile, CompressionMode.Compress))
                    {
                        file.CopyTo(zip);
                    }
                }
            }
        }

        public string Get(ushort index) => strings[index];

        public void AddRange(IEnumerable<string> items) => strings.AddRange(items);
    }
}