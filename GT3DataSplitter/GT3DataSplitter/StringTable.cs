using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StreamExtensions;

namespace GT3.DataSplitter
{
    public class StringTable
    {
        public List<string> Strings = new List<string>();
        public List<string> UnusedStrings;
        public StringTableLookup Lookup { get; set; }

        public StringTable() => Lookup = new StringTableLookup(this);

        public void Read(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] magic = new byte[4];
                file.Read(magic);
                if (Encoding.ASCII.GetString(magic) != "STDB")
                {
                    Console.WriteLine("Not a STDB file.");
                    return;
                }

                uint stringCount = file.ReadUInt();
                uint stringType = file.ReadUInt();
                Encoding encoding = Encoding.Default;
                if (stringType == 0xFFFF)
                {
                    encoding = Encoding.GetEncoding("EUC-JP");
                }
                else if (stringType != 0x0001)
                {
                    Console.WriteLine("Unknown string type.");
                    return;
                }

                if (file.Length != file.ReadUInt())
                {
                    Console.WriteLine("File length incorrect.");
                }

                for (int i = 0; i < stringCount; i++)
                {
                    uint stringPosition = file.ReadUInt();
                    long storedPosition = file.Position;
                    file.Position = stringPosition;
                    ushort stringLength = file.ReadUShort();
                    byte[] stringBytes = new byte[stringLength];
                    file.Read(stringBytes);
                    Strings.Add(encoding.GetString(stringBytes).TrimEnd('\0'));
                    file.Position = storedPosition;
                }
            }

            UnusedStrings = new List<string>(Strings);
        }

        public void Export(string filename)
        {
            string directory = "Strings";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            using (TextWriter output = new StreamWriter(File.Create($"{directory}\\{filename}.csv"), Encoding.UTF8))
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

        public void Import(string filename)
        {
            filename = $"Strings\\{filename}.csv";

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

        public void Write(string filename, bool unicode)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                file.WriteCharacters("STDB");
                file.WriteUInt((uint)Strings.Count);
                file.WriteUInt((uint)(unicode ? 0xFFFF : 0x01));
                file.Position += 4;

                file.Position += Strings.Count * 4;

                int i = 0;
                foreach (string newString in Strings)
                {
                    uint startPosition = (uint)file.Position;
                    byte[] characters = (unicode ? Encoding.GetEncoding("EUC-JP") : Encoding.ASCII).GetBytes((newString + "\0").ToCharArray());
                    ushort length = (ushort)characters.Length;
                    if (unicode)
                    {
                        length += (ushort)(length % 2);
                    }
                    else
                    {
                        length -= 1;
                    }
                    file.WriteUShort(length);
                    file.Write(characters, 0, characters.Length);
                    if (file.Position % 2 != 0)
                    {
                        file.WriteByte(0);
                    }

                    file.Position = 0x10 + (4 * i++);
                    file.WriteUInt(startPosition);
                    file.Position = file.Length;
                }

                file.Position = 0x0C;
                file.WriteUInt((uint)file.Length);
            }
        }

        public ushort Add(string text)
        {
            if (Strings.Contains(text))
            {
                return (ushort)Strings.IndexOf(text);
            }

            Strings.Add(text);
            return (ushort)(Strings.Count - 1);
        }

        public string Get(ushort index)
        {
            if (UnusedStrings[index] != "-")
            {
                UnusedStrings[index] = null;
            }
            return Strings[index];
        }

        public class StringTableLookup : ITypeConverter
        {
            private readonly StringTable table;

            public StringTableLookup(StringTable table) => this.table = table;

            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return table.Add(text);
            }

            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                return table.Get(Convert.ToUInt16(value));
            }
        }
    }
}
