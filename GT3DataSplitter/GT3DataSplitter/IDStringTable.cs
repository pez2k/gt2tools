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
    using HashGenerator;

    public class IDStringTable
    {
        private readonly SortedDictionary<ulong, string> ids = new SortedDictionary<ulong, string>();
        private readonly StringTable stringTable = new StringTable();
        public IDStringTableLookup Lookup { get; set; }

        private const string Filename = "IDStrings";

        public IDStringTable() => Lookup = new IDStringTableLookup(this);

        public void Read(string indexFilename, string stringFilename)
        {
            stringTable.Read(stringFilename);

            using (FileStream file = new FileStream(indexFilename, FileMode.Open, FileAccess.Read))
            {
                byte[] magic = new byte[4];
                file.Read(magic);
                if (Encoding.ASCII.GetString(magic) != "IDDB")
                {
                    Console.WriteLine("Not an IDDB file.");
                    return;
                }

                uint idCount = file.ReadUInt();
                for (int i = 0; i < idCount; i++)
                {
                    ulong id = file.ReadULong();
                    ushort num = (ushort)file.ReadULong();
                    ids.Add(id, stringTable.Get(num));
                }
            }
        }

        public void Export()
        {
            stringTable.UnusedStrings = stringTable.Strings;
            stringTable.Export(Filename);
        }

        public string Get(ulong id) => ids.TryGetValue(id, out string value) ? value : null;

        public ulong Add(string name)
        {
            ulong hash = HashGenerator.GenerateHash(name);
            if (!string.IsNullOrEmpty(name) && !ids.ContainsValue(name))
            {
                ids.Add(hash, name);
            }
            return hash;
        }

        public void Import()
        {
            stringTable.Import(Filename);
            foreach (string name in stringTable.Strings)
            {
                Add(name);
            }
        }

        public void Write()
        {
            using (FileStream file = new FileStream(".id_db_idx_eu.db", FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("IDDB");
                file.WriteUInt((uint)ids.Count);
                ulong i = 0;
                foreach (ulong hash in ids.Keys)
                {
                    file.WriteULong(hash);
                    file.WriteULong(i++);
                }
            }

            stringTable.Strings.Clear();
            foreach (string name in ids.Values)
            {
                stringTable.Add(name);
            }
            stringTable.Write(".id_db_str_eu.db", false);
        }

        public void Write_jp()
        {
            using (FileStream file = new FileStream(".id_db_idx.db", FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("IDDB");
                file.WriteUInt((uint)ids.Count);
                ulong i = 0;
                foreach (ulong hash in ids.Keys)
                {
                    file.WriteULong(hash);
                    file.WriteULong(i++);
                }
            }

            stringTable.Strings.Clear();
            foreach (string name in ids.Values)
            {
                stringTable.Add(name);
            }
            stringTable.Write(".id_db_str.db", false);
        }

        public class IDStringTableLookup : ITypeConverter
        {
            private readonly IDStringTable table;

            public IDStringTableLookup(IDStringTable table) => this.table = table;

            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return 0;
            }

            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                return table.Get(Convert.ToUInt64(value));
            }
        }
    }
}