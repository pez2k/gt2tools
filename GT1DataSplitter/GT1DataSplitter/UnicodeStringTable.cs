using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using CsvHelper;

namespace GT1.DataSplitter
{
    using StreamExtensions;

    public static class UnicodeStringTable
    {
        private static List<string> strings = new();
        private static List<string> unusedStrings;
        private static readonly Encoding binaryEncoding = Encoding.Unicode;
        private static readonly Encoding textEncoding = Encoding.UTF8;

        public static void Read(string filename)
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

            unusedStrings = new List<string>(strings);
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

        public static void Export()
        {
            string directory = CreateLanguageDirectory();

            using (TextWriter output = new StreamWriter(File.Create($"{directory}\\PartStrings.csv"), textEncoding))
            {
                using (CsvWriter csv = new(output, Program.CSVConfig))
                {
                    for (int i = 0; i < unusedStrings.Count; i++)
                    {
                        if (unusedStrings[i] == null)
                        {
                            continue;
                        }
                        csv.WriteField(i.ToString());
                        csv.WriteField(unusedStrings[i]);
                        csv.NextRecord();
                    }
                }
            }
        }

        public static string CreateLanguageDirectory()
        {
            string directory = $"Strings\\{Program.LanguagePrefix}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }

        public static void Import()
        {
            string filename = $"Strings\\{Program.LanguagePrefix}\\PartStrings.csv";

            using (TextReader input = new StreamReader(filename, textEncoding))
            {
                using (CsvReader csv = new(input, Program.CSVConfig))
                {
                    while (csv.Read())
                    {
                        strings.Add(csv.GetField(1));
                    }
                }
            }
        }

        public static void Write(string filename)
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

        public static ushort Add(string text)
        {
            if (strings.Contains(text))
            {
                return (ushort)strings.IndexOf(text);
            }

            strings.Add(text);
            return (ushort)(strings.Count - 1);
        }

        public static string Get(ushort index)
        {
            if (unusedStrings[index] != "-")
            {
                unusedStrings[index] = null;
            }
            return strings[index];
        }

        public static void Reset()
        {
            strings.Clear();
            unusedStrings?.Clear();
        }

        public static void LoadFromArray(string[] stringArray)
        {
            strings = stringArray.ToList();
            unusedStrings = stringArray.ToList();
        }
    }
}