using System;
using System.IO;
using System.IO.Compression;

namespace GT2.DataSplitter
{
    class Program
    {
        public static string LanguagePrefix { get; set; }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                BuildFile();
                return;
            }

            string filename = Path.GetFileName(args[0]);
            string extension = Path.GetExtension(filename);

            //hack
            if (filename.Contains("license_data"))
            {
                SplitLicenseFile(filename);
                return;
            }
            else if (filename.Contains("arcade_data"))
            {
                SplitArcadeFile(filename);
                return;
            }

            if (extension == ".gz")
            {
                string innerFilename = Path.GetFileNameWithoutExtension(filename);
                extension = Path.GetExtension(innerFilename);

                if (extension != ".dat")
                {
                    return;
                }

                using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    // Un-gzip
                    using (GZipStream unzip = new GZipStream(infile, CompressionMode.Decompress))
                    {
                        filename = innerFilename;
                        using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                        {
                            unzip.CopyTo(outfile);
                        }
                    }
                }
            }

            if (extension == ".dat")
            {
                LanguagePrefix = filename.Split('_')[0];
                if (LanguagePrefix.Length != 3)
                {
                    LanguagePrefix = "jpn";
                }
                SplitFile();
            }
        }

        static void SplitFile()
        {
            StringTable.Read($"{LanguagePrefix}_unistrdb.dat");

            GTModeData CarData = new GTModeData();
            CarData.ReadData($"{LanguagePrefix}_gtmode_data.dat");
            CarData.DumpData();

            GTModeRace RaceData = new GTModeRace();
            RaceData.ReadData($"{LanguagePrefix}_gtmode_race.dat");
            RaceData.DumpData();

            StringTable.Export();
            CarNameStringTable.Export();
        }

        static void BuildFile()
        {
            var languageDirectories = Directory.GetDirectories("Strings");
            foreach (string languageDirectory in languageDirectories)
            {
                string language = languageDirectory.Split('\\')[1];
                Console.WriteLine($"Building language '{language}'...");

                LanguagePrefix = language;
                StringTable.Import();

                if (!File.Exists($"Strings\\{LanguagePrefix}\\CarNames.csv"))
                {
                    LanguagePrefix = "eng";
                }

                CarNameStringTable.Import();
                LanguagePrefix = language;

                GTModeData CarData = new GTModeData();
                CarData.ImportData();
                CarData.WriteData($"{LanguagePrefix}_gtmode_data.dat");

                GTModeRace RaceData = new GTModeRace();
                RaceData.ImportData();
                RaceData.WriteData($"{LanguagePrefix}_gtmode_race.dat");

                StringTable.Write($"{LanguagePrefix}_unistrdb.dat");

                StringTable.Reset();
                CarNameStringTable.Reset();
            }
        }

        static void SplitLicenseFile(string filename)
        {
            StringTable.Read("eng_unistrdb.dat");
            LicenseData LicenseData = new LicenseData();
            LicenseData.ReadData(filename);
            LicenseData.DumpData();
            StringTable.Export();
            CarNameStringTable.Export();
        }

        static void SplitArcadeFile(string filename)
        {
            StringTable.Read("eng_unistrdb.dat");
            ArcadeData ArcadeData = new ArcadeData();
            ArcadeData.ReadData(filename);
            ArcadeData.DumpData();
            StringTable.Export();
            CarNameStringTable.Export();
        }
    }
}
