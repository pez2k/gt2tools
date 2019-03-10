using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GT2.DataSplitter
{
    public static class CarNameStringTable
    {
        public static Dictionary<string, (string NameFirstPart, string NameSecondPart)> Strings = new Dictionary<string, (string, string)>();

        public static void Add(string carID, string nameFirstPart, string nameSecondPart)
        {
            Strings.Add(carID, (nameFirstPart, nameSecondPart));
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
                    foreach (var carName in Strings)
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.WriteField(carName.Key);
                        csv.WriteField(carName.Value.NameFirstPart);
                        csv.WriteField(carName.Value.NameSecondPart);
                        csv.NextRecord();
                    }
                }
            }
        }

        public static void Import()
        {
            string filename = $"Strings\\{Program.LanguagePrefix}\\CarNames.csv";

            using (TextReader input = new StreamReader(filename, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(input))
                {
                    while (csv.Read())
                    {
                        Strings.Add(csv.GetField(0), (csv.GetField(1), csv.GetField(2)));
                    }
                }
            }
        }

        public static (string nameFirstPart, string nameSecondPart) Get(string carID)
        {
            return Strings.TryGetValue(carID, out (string nameFirstPart, string nameSecondPart) carName)? carName : throw new Exception($"Car name for {carID} not found.");
        }

        public static void Reset()
        {
            Strings.Clear();
        }
    }
}