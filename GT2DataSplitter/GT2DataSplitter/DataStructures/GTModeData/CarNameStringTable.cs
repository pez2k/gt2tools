using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;

namespace GT2.DataSplitter
{
    public static class CarNameStringTable
    {
        private static readonly List<CarName> strings = new List<CarName>();
        private static readonly List<CarName> defaultStrings = new List<CarName>();

        public static void Add(uint carID, string nameFirstPart, string nameSecondPart, byte year) =>
            strings.Add(new CarName { CarID = carID, NameFirstPart = nameFirstPart, NameSecondPart = nameSecondPart, Year = year });

        public static void Export()
        {
            string directory = $"Strings\\{Program.LanguagePrefix}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (TextWriter output = new StreamWriter(File.Create($"{directory}\\CarNames.csv"), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(output, Program.CSVConfig))
                {
                    csv.Context.RegisterClassMap<CarNameCSVMap>();
                    csv.WriteHeader<CarName>();
                    csv.NextRecord();

                    foreach (CarName carName in strings)
                    {
                        csv.WriteRecord(carName);
                        csv.NextRecord();
                    }
                }
            }
        }

        public static void Import()
        {
            Import(Program.LanguagePrefix, strings);
            Import("eng", defaultStrings);
        }

        private static void Import(string languagePrefix, List<CarName> strings)
        {
            string directory = $"Strings\\{languagePrefix}";
            if (!Directory.Exists(directory))
            {
                return;
            }

            string carNamesPath = $"{directory}\\CarNames.csv";
            if (File.Exists(carNamesPath))
            {
                ImportCSV(carNamesPath, strings);
            }

            var filenames = Directory.EnumerateFiles(directory, "*.csv").Where(file => Path.GetFileName(file) != "CarNames.csv" && Path.GetFileName(file) != "PartStrings.csv");

            foreach (string filename in filenames)
            {
                ImportCSV(filename, strings);
            }
        }

        private static void ImportCSV(string filename, List<CarName> strings)
        {
            using (TextReader input = new StreamReader(filename, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(input, Program.CSVConfig))
                {
                    csv.Context.RegisterClassMap<CarNameCSVMap>();
                    csv.Read();
                    csv.ReadHeader();
                    
                    while (csv.Read())
                    {
                        CarName carName = csv.GetRecord<CarName>();
                        strings.RemoveAll(existingCarName => existingCarName.CarID == carName.CarID);
                        strings.Add(carName);
                    }
                }
            }
        }

        public static CarName Get(uint carID) => strings.SingleOrDefault(carName => carName.CarID == carID) ?? GetDefault(carID);

        private static CarName GetDefault(uint carID)
        {
            CarName defaultName = defaultStrings.SingleOrDefault(carName => carName.CarID == carID) ?? throw new Exception($"Car name for {carID} not found.");
            strings.Add(defaultName);
            return defaultName;

        }

        public static void Reset()
        {
            strings.Clear();
            defaultStrings.Clear();
        }
    }
}