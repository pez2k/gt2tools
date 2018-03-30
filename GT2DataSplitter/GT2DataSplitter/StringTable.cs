using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GT2.DataSplitter
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;
    using StreamExtensions;
    using System.IO.Compression;

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
            string directory = "Strings";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            string filename = directory + "\\_cache.dat";
            using (TextWriter output = new StreamWriter(File.Create(filename), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(output))
                {
                    for (int i = 0; i < Strings.Count; i++)
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.WriteField(i.ToString());
                        csv.WriteField(Strings[i]);
                        csv.NextRecord();
                    }
                }
            }

            filename = directory + "\\PartStrings.csv";
            using (TextWriter output = new StreamWriter(File.Create(filename), Encoding.UTF8))
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
            string directory = "Strings";
            string filename = directory + "\\PartStrings.csv";

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
            filename = "new_" + filename;

            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                byte[] header = { 0xCA, 0x52, 0x00, 0x00, 0x57, 0x53, 0x44, 0x42 };
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
                using (FileStream zipFile = new FileStream(filename + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new GZipStream(zipFile, CompressionMode.Compress))
                    {
                        file.CopyTo(zip);
                    }
                }
            }
        }

        public class StringTableLookup : ITypeConverter
        {
            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (Strings.Contains(text))
                {
                    return (ushort)Strings.IndexOf(text);
                }
                
                Strings.Add(text);
                return (ushort)(Strings.Count - 1);
            }

            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                ushort i = Convert.ToUInt16(value);
                if (UnusedStrings[i] != "-")
                {
                    UnusedStrings[i] = null;
                }
                return Strings[i];
            }
        }
    }
}
