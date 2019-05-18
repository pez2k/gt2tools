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

    public class IDStringTable
    {
        private readonly Dictionary<ulong, string> ids = new Dictionary<ulong, string>();
        private readonly StringTable stringTable = new StringTable();
        public IDStringTableLookup Lookup { get; set; }

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

        public string Get(ulong id) => ids[id];

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