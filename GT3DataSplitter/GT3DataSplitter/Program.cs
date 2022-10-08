using System.IO;

namespace GT3.DataSplitter
{
    class Program
    {
        public static IDStringTable IDStrings = new IDStringTable();
        public static StringTable Strings = new StringTable();
        public static StringTable UnicodeStrings = new StringTable();
        public static StringTable ColourStrings = new StringTable();

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                BuildFile();
                return;
            }

            string filename = Path.GetFileName(args[0]);
            string extension = Path.GetExtension(filename);
            
            if (extension == ".db")
            {
                if (filename.Contains("_gtc"))
                {
                    SplitFileGTC();
                    return;
                }
                SplitFile();
            }
        }

        static void SplitFile()
        {
            string currentdirectory = Directory.GetCurrentDirectory();
            bool Spliteu = true;

            if (File.Exists(currentdirectory + @"\.id_db_idx_eu.db") && File.Exists(currentdirectory + @"\.id_db_str_eu.db"))
                IDStrings.Read(".id_db_idx_eu.db", ".id_db_str_eu.db");
            else if (File.Exists(currentdirectory + @"\.id_db_idx.db") && File.Exists(currentdirectory + @"\.id_db_str.db"))
            {
                IDStrings.Read(".id_db_idx.db", ".id_db_str.db");
                Spliteu = false;
            }

            if (File.Exists(currentdirectory + @"\paramstr_eu.db"))
                Strings.Read("paramstr_eu.db");
            else if (File.Exists(currentdirectory + @"\paramstr.db"))
            {
                Strings.Read("paramstr.db");
                Spliteu = false;
            }

            if (File.Exists(currentdirectory + @"\paramstr_eu.db"))
                UnicodeStrings.Read("paramunistr_eu.db");
            else if (File.Exists(currentdirectory + @"\paramstr.db"))
            {
                UnicodeStrings.Read("paramunistr.db");
                Spliteu = false;
            }

            ColourStrings.Read("carcolor.sdb");

            var database = new ParamDB();

            if (File.Exists(currentdirectory + @"\paramdb_eu.db"))
                database.ReadData("paramdb_eu.db");
            else if (File.Exists(currentdirectory + @"\paramdb.db"))
            {
                database.ReadData("paramdb.db");
                Spliteu = false;
            }

            var raceDetails = new RaceDetailDB();
            raceDetails.ReadData("racedetail.db");

            var raceModes = new RaceModeDB();
            raceModes.ReadData("racemode.db");

            if (Spliteu == true)
            {
                Directory.CreateDirectory("Data");
                Directory.SetCurrentDirectory("Data");
                database.DumpData();
                raceDetails.DumpData();
                raceModes.DumpData();

                Strings.Export("Strings");
                UnicodeStrings.Export("UnicodeStrings");
                ColourStrings.Export("ColourStrings");
                IDStrings.Export();
            }

            else if (Spliteu == false)
            {
                Directory.CreateDirectory("Data_jp");
                Directory.SetCurrentDirectory("Data_jp");
                database.DumpData();
                raceDetails.DumpData();
                raceModes.DumpData();

                Strings.Export("Strings");
                UnicodeStrings.Export("UnicodeStrings");
                ColourStrings.Export("ColourStrings");
                IDStrings.Export();
            }
        }

        static void BuildFile()
        {
            string currentdirectory = Directory.GetCurrentDirectory();

            var database = new ParamDB();

            if (Directory.Exists(currentdirectory + @"\Data"))
            {
                Directory.SetCurrentDirectory("Data");
                IDStrings.Import();
                Strings.Import("Strings");
                UnicodeStrings.Import("UnicodeStrings");
                ColourStrings.Import("ColourStrings");
                database.ImportData();
                Directory.CreateDirectory("..\\Output");
                Directory.SetCurrentDirectory("..\\Output");
                database.WriteData();
                IDStrings.Write();
                Strings.Write("paramstr_eu.db", false);
                UnicodeStrings.Write("paramunistr_eu.db", true);
                ColourStrings.Write("carcolor.sdb", true);
            }

            else if (Directory.Exists(currentdirectory + @"\Data_jp"))
            {
                Directory.SetCurrentDirectory("Data_jp");
                IDStrings.Import();
                Strings.Import("Strings");
                UnicodeStrings.Import("UnicodeStrings");
                ColourStrings.Import("ColourStrings");
                database.ImportData();
                Directory.CreateDirectory("..\\Output_jp");
                Directory.SetCurrentDirectory("..\\Output_jp");
                database.WriteData_jp();
                IDStrings.Write_jp();
                Strings.Write("paramstr.db", false);
                UnicodeStrings.Write("paramunistr.db", true);
                ColourStrings.Write("carcolor.sdb", true);
            }
        }

        static void SplitFileGTC()
        {
            string currentdirectory = Directory.GetCurrentDirectory();
            bool Spliteu = true;

            if (File.Exists(currentdirectory + @"\.id_db_idx_gtc_eu.db") && File.Exists(currentdirectory + @"\.id_db_str_gtc_eu.db"))
                IDStrings.Read(".id_db_idx_gtc_eu.db", ".id_db_str_gtc_eu.db");

            else if (File.Exists(currentdirectory + @"\.id_db_idx.db") && File.Exists(currentdirectory + @"\.id_db_str.db"))
            {
                IDStrings.Read(".id_db_idx.db", ".id_db_str.db");
                Spliteu = false;
            }
            else if (File.Exists(currentdirectory + @"\.id_db_idx_gtc.db") && File.Exists(currentdirectory + @"\.id_db_str_gtc.db"))
            {
                IDStrings.Read(".id_db_idx_gtc.db", ".id_db_str_gtc.db");
                Spliteu = false;
            }

            if (File.Exists(currentdirectory + @"\paramstr_gtc_eu.db"))
                Strings.Read("paramstr_gtc_eu.db");

            else if (File.Exists(currentdirectory + @"\paramstr.db"))
            {
                Strings.Read("paramstr.db");
                Spliteu = false;
            }
            else if (File.Exists(currentdirectory + @"\paramstr_gtc.db"))
            {
                Strings.Read("paramstr_gtc.db");
                Spliteu = false;
            }

            if (File.Exists(currentdirectory + @"\paramunistr_gtc_eu.db"))
                UnicodeStrings.Read("paramunistr_gtc_eu.db");

            else if (File.Exists(currentdirectory + @"\paramunistr.db"))
            {
                UnicodeStrings.Read("paramunistr.db");
                Spliteu = false;
            }
            else if (File.Exists(currentdirectory + @"\paramunistr_gtc.db"))
            {
                UnicodeStrings.Read("paramunistr_gtc.db");
                Spliteu = false;
            }

            ColourStrings.Read("carcolor.sdb");

            var database = new ParamDBConcept();

            if (File.Exists(currentdirectory + @"\paramdb_gtc_eu.db"))
                database.ReadData("paramdb_gtc_eu.db");

            else if (File.Exists(currentdirectory + @"\paramdb.db"))
            {
                database.ReadData("paramdb.db");
                Spliteu = false;
            }
            else if (File.Exists(currentdirectory + @"\paramdb_gtc.db"))
            {
                database.ReadData("paramdb_gtc.db");
                Spliteu = false;
            }

            var raceDetails = new RaceDetailDB();
            raceDetails.ReadData("racedetail.db");

            var raceModes = new RaceModeDB();
            raceModes.ReadData("racemode.db");

            if (Spliteu == true)
            {
                Directory.CreateDirectory("Data");
                Directory.SetCurrentDirectory("Data");
                database.DumpData();
                raceDetails.DumpData();
                raceModes.DumpData();

                Strings.Export("Strings");
                UnicodeStrings.Export("UnicodeStrings");
                ColourStrings.Export("ColourStrings");
                IDStrings.Export();
            }

            else if (Spliteu == false)
            {
                Directory.CreateDirectory("Data_jp");
                Directory.SetCurrentDirectory("Data_jp");
                database.DumpData();
                raceDetails.DumpData();
                raceModes.DumpData();

                Strings.Export("Strings");
                UnicodeStrings.Export("UnicodeStrings");
                ColourStrings.Export("ColourStrings");
                IDStrings.Export();
            }
        }
    }
}