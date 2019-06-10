using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GT2.DataSplitter
{
    using StreamExtensions;

    public static class StringTable
    {
        public static List<string> Strings = new List<string>();
        public static List<string> UnusedStrings;
        public static StringTableLookup Lookup { get; set; } = new StringTableLookup();

        public static void Read(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Position = 8;
                ushort stringCount = file.ReadUShort();

                for (ushort i = 0; i < stringCount; i++)
                {
                    int length = (file.ReadUShort() + 1) * 2;
                    byte[] characterData = new byte[length];
                    file.Read(characterData, 0, length);

                    Strings.Add(Encoding.Unicode.GetString(characterData).TrimEnd('\0'));
                }
            }

            UnusedStrings = new List<string>(Strings);
        }

        public static void Export()
        {
            string directory = $"Strings\\{Program.LanguagePrefix}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (TextWriter output = new StreamWriter(File.Create($"{directory}\\PartStrings.csv"), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(output))
                {
                    for (int i = 0; i < UnusedStrings.Count; i++)
                    {
                        if (UnusedStrings[i] == null)
                        {
                            continue;
                        }
                        csv.Configuration.QuoteAllFields = true;
                        csv.WriteField(i.ToString());
                        csv.WriteField(UnusedStrings[i]);
                        csv.NextRecord();
                    }
                }
            }
        }

        public static void Import()
        {
            string filename = $"Strings\\{Program.LanguagePrefix}\\PartStrings.csv";

            using (TextReader input = new StreamReader(filename, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(input))
                {
                    while (csv.Read())
                    {
                        Strings.Add(csv.GetField(1));
                    }
                }
            }
        }

        public static void Write(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                byte[] header = { 0x00, 0x00, 0x00, 0x00, 0x57, 0x53, 0x44, 0x42 };
                file.Write(header, 0, header.Length);
                file.WriteUShort((ushort)Strings.Count);

                foreach (string newString in Strings)
                {
                    byte[] characters = Encoding.Unicode.GetBytes((newString + "\0").ToCharArray());
                    ushort length = (ushort)((characters.Length - 1) / 2);
                    file.WriteUShort(length);
                    file.Write(characters, 0, characters.Length);
                }

                file.Position = 0;
                file.WriteUShort((ushort)file.Length);

                file.Position = 0;
                using (FileStream zipFile = new FileStream(filename + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new GZipStream(zipFile, CompressionMode.Compress))
                    {
                        file.CopyTo(zip);
                    }
                }
            }
        }

        public static ushort Add(string text)
        {
            if (Strings.Contains(text))
            {
                return (ushort)Strings.IndexOf(text);
            }

            Strings.Add(text);
            return (ushort)(Strings.Count - 1);
        }

        public static string Get(ushort index)
        {
            if (UnusedStrings[index] != "-")
            {
                UnusedStrings[index] = null;
            }
            return Strings[index];
        }

        public static void Reset()
        {
            Strings.Clear();
            UnusedStrings?.Clear();
        }

        public class StringTableLookup : ITypeConverter
        {
            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return Add(text);
            }

            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                return Get(Convert.ToUInt16(value));
            }
        }
    }
}
