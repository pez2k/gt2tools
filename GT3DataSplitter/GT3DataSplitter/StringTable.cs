using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GT3.DataSplitter
{
    using StreamExtensions;

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
