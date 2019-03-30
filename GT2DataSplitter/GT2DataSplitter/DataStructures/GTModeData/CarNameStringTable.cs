using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GT2.DataSplitter
{
    public static class CarNameStringTable
    {
        public static List<CarName> Strings = new List<CarName>();

        public static void Add(uint carID, string nameFirstPart, string nameSecondPart, byte year)
        {
            Strings.Add(new CarName { CarID = carID, NameFirstPart = nameFirstPart, NameSecondPart = nameSecondPart, Year = year });
        }

        public static void Export()
        {
            string directory = $"Strings\\{Program.LanguagePrefix}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (TextWriter output = new StreamWriter(File.Create($"{directory}\\CarNames.csv"), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(output))
                {
                    csv.Configuration.RegisterClassMap<CarNameCSVMap>();
                    csv.Configuration.QuoteAllFields = true;
                    csv.WriteHeader<CarName>();
                    csv.NextRecord();

                    foreach (CarName carName in Strings)
                    {
                        csv.WriteRecord(carName);
                        csv.NextRecord();
                    }
                }
            }
        }

        public static void Import()
        {
            string directory = $"Strings\\{Program.LanguagePrefix}";
            ImportCSV($"{directory}\\CarNames.csv");

            var filenames = Directory.EnumerateFiles(directory, "*.csv").Where(file => Path.GetFileName(file) != "CarNames.csv" && Path.GetFileName(file) != "PartStrings.csv");

            foreach (string filename in filenames)
            {
                ImportCSV(filename);
            }
        }

        private static void ImportCSV(string filename)
        {
            using (TextReader input = new StreamReader(filename, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(input))
                {
                    csv.Configuration.RegisterClassMap<CarNameCSVMap>();
                    csv.Read();
                    csv.ReadHeader();
                    
                    while (csv.Read())
                    {
                        CarName carName = csv.GetRecord<CarName>();
                        Strings.RemoveAll(existingCarName => existingCarName.CarID == carName.CarID);
                        Strings.Add(carName);
                    }
                }
            }
        }

        public static CarName Get(uint carID)
        {
            return Strings.SingleOrDefault(carName => carName.CarID == carID) ?? throw new Exception($"Car name for {carID} not found.");
        }

        public static void Reset()
        {
            Strings.Clear();
        }
    }
}