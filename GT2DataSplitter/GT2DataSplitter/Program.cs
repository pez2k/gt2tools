using System;
using System.IO;
using System.IO.Compression;

namespace GT2.DataSplitter
{
    class Program
    {
        public static string LanguagePrefix { get; private set; }
        public static string OverridePath { get; private set; }

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

            var carData = new GTModeData();
            carData.ReadData($"{GetDataFilePrefix()}gtmode_data.dat");
            carData.DumpData();

            GTModeRace raceData = new GTModeRace();
            raceData.ReadData($"{GetDataFilePrefix()}gtmode_race.dat");
            raceData.DumpData();

            StringTable.Export();
            CarNameStringTable.Export();
        }

        static void BuildFile()
        {
            var languageDirectories = Directory.GetDirectories("Strings");
            foreach (string languageDirectory in languageDirectories)
            {
                LanguagePrefix = languageDirectory.Split('\\')[1];
                Console.WriteLine($"Building language '{LanguagePrefix}'...");

                StringTable.Import();
                CarNameStringTable.Import();

                string overridePath = Path.Combine("_Overrides", LanguagePrefix);
                OverridePath = Directory.Exists(overridePath) ? overridePath : null;

                var carData = new GTModeData();
                carData.ImportData();
                Directory.CreateDirectory("Output");
                carData.WriteData(Path.Combine("Output", $"{GetDataFilePrefix()}gtmode_data.dat"));

                GTModeRace raceData = new GTModeRace();
                raceData.ImportData();
                raceData.WriteData(Path.Combine("Output", $"{GetDataFilePrefix()}gtmode_race.dat"));

                StringTable.Write(Path.Combine("Output", $"{LanguagePrefix}_unistrdb.dat"));

                StringTable.Reset();
                CarNameStringTable.Reset();
            }
        }

        static string GetDataFilePrefix() => LanguagePrefix == "jpn" ? "" : $"{LanguagePrefix}_";

        static void SplitLicenseFile(string filename)
        {
            StringTable.Read("eng_unistrdb.dat");
            LicenseData licenseData = new LicenseData();
            licenseData.ReadData(filename);
            licenseData.DumpData();
            StringTable.Export();
            CarNameStringTable.Export();
        }

        static void SplitArcadeFile(string filename)
        {
            StringTable.Read("eng_unistrdb.dat");
            ArcadeData arcadeData = new ArcadeData();
            arcadeData.ReadData(filename);
            arcadeData.DumpData();
            StringTable.Export();
            CarNameStringTable.Export();
        }
    }
}