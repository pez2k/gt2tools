using System.IO;

namespace GT3.DataSplitter
{
    class Program
    {
        public static IDStringTable IDStrings = new IDStringTable();
        public static StringTable Strings = new StringTable();

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            string filename = Path.GetFileName(args[0]);
            string extension = Path.GetExtension(filename);
            
            if (extension == ".db")
            {
                SplitFile();
            }
        }

        static void SplitFile()
        {
            IDStrings.Read(".id_db_idx_eu.db", ".id_db_str_eu.db");
            Strings.Read("paramunistr_eu.db");

            var database = new ParamDB();
            database.ReadData("paramdb_eu.db");
            database.DumpData();

            Strings.Export("UnicodeStrings");
        }
    }
}