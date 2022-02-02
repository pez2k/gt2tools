using System;
using System.IO;

namespace GT2.DataSplitter
{
    class Program
    {
        public static string LanguagePrefix { get; private set; }

        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                BuildGTModeFile();
                return;
            }

            string filename = Path.GetFileName(args[0]);
            string extension = Path.GetExtension(filename);
            bool favourCompressed = false;

            if (extension == ".gz")
            {
                string innerFilename = Path.GetFileNameWithoutExtension(filename);
                extension = Path.GetExtension(innerFilename);
                favourCompressed = true;
            }
            if (extension != ".dat")
            {
                return;
            }
            SetLanguagePrefix(filename);
            StringTable.Read(GetCorrectFilename($"{LanguagePrefix}_unistrdb.dat", favourCompressed));

            if (filename.Contains("license_data"))
            {
                DumpDataFile<LicenseData>("license_data.dat", favourCompressed);
            }
            else if (filename.Contains("arcade_data"))
            {
                DumpDataFile<ArcadeData>("arcade_data.dat", favourCompressed);
            }
            else
            {
                DumpDataFile<GTModeData>("gtmode_data.dat", favourCompressed);
                DumpDataFile<GTModeRace>("gtmode_race.dat", favourCompressed);
            }
            StringTable.Export();
            CarNameStringTable.Export();
        }

        private static void DumpDataFile<TData>(string filename, bool favourCompressed) where TData : DataFile, new()
        {
            var data = new TData();
            data.ReadData(GetCorrectFilename($"{GetDataFilePrefix()}{filename}", favourCompressed));
            data.DumpData();
        }

        private static string GetCorrectFilename(string filename, bool favourCompressed)
        {
            string compressedFilename = $"{filename}.gz";
            return File.Exists(compressedFilename) && (favourCompressed || !File.Exists(filename)) ? compressedFilename : filename;
        }

        private static void BuildGTModeFile()
        {
            var languageDirectories = Directory.GetDirectories("Strings");
            foreach (string languageDirectory in languageDirectories)
            {
                LanguagePrefix = languageDirectory.Split('\\')[1];
                Console.WriteLine($"Building language '{LanguagePrefix}'...");

                StringTable.Import();
                CarNameStringTable.Import();

                string overridePath = Path.Combine("_Overrides", LanguagePrefix);
                DataFile.OverridePath = Directory.Exists(overridePath) ? overridePath : null;

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

        private static void SetLanguagePrefix(string filename)
        {
            LanguagePrefix = filename.Split('_')[0];
            if (LanguagePrefix.Length != 3)
            {
                LanguagePrefix = "jpn";
            }
        }

        private static string GetDataFilePrefix() => LanguagePrefix == "jpn" ? "" : $"{LanguagePrefix}_";
    }
}