using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using StreamExtensions;

namespace GT3.CarColorEditor
{
    using HashGenerator;

    public class CarAndColourList
    {
        private readonly Dictionary<ulong, string> modelIDs = new Dictionary<ulong, string>() // Initialised with IDs that may appear in the file but aren't in any ID table
        {
            [HashGenerator.GenerateHash("ar0019")] = "ar0019",
            [HashGenerator.GenerateHash("fo0016")] = "fo0016",
            [HashGenerator.GenerateHash("fo0024")] = "fo0024",
            [HashGenerator.GenerateHash("ho0097")] = "ho0097",
            [HashGenerator.GenerateHash("ma0097")] = "ma0097",
            [HashGenerator.GenerateHash("ml0001")] = "ml0001",
            [HashGenerator.GenerateHash("ni0120")] = "ni0120",
            [HashGenerator.GenerateHash("pd0001")] = "pd0001",
            [HashGenerator.GenerateHash("ro0003")] = "ro0003",
            [HashGenerator.GenerateHash("wl1003")] = "wl1003"
        };

        public List<Car> Cars { get; set; } = new List<Car>();
        public SortedDictionary<uint, CarColour> Colours { get; set; } = new SortedDictionary<uint, CarColour>();

        public void ReadFromGameFiles()
        {
            using (var file = new FileStream("carcolor.db", FileMode.Open, FileAccess.Read))
            {
                if (!file.ReadULong().ToByteArray().SequenceEqual("GT2K\0\0\0\0".ToByteArray()))
                {
                    throw new Exception("Invalid header.");
                }
                uint carCount = file.ReadUInt();
                uint carToColoursMapIndex = file.ReadUInt();
                uint colourListIndex = file.ReadUInt();
                uint fileSize = file.ReadUInt();
                if (fileSize != file.Length)
                {
                    throw new Exception("Invalid file size.");
                }

                long carListIndex = file.Position;
                file.Position = colourListIndex;
                uint coloursCount = file.ReadUInt();

                var colourNames = new StringTable();
                colourNames.Read("carcolor.sdb");

                for (uint i = 0; i < coloursCount; i++)
                {
                    var colour = new CarColour();
                    colour.ReadFromGameFiles(file, colourNames);
                    Colours.Add(colour.ColourID, colour);
                }

                file.Position = carListIndex;
                PopulateModelIDs();

                for (uint i = 0; i < carCount; i++)
                {
                    var car = new Car();
                    car.ReadFromGameFiles(file, carToColoursMapIndex, modelIDs, Colours);
                    Cars.Add(car);
                }
            }
        }

        private void PopulateModelIDs()
        {
            var suffixes = new string[] { "", "_eu", "_us" };
            foreach (string suffix in suffixes)
            {
                string indexFile = $".id_db_idx{suffix}.db";
                string stringFile = $".id_db_str{suffix}.db";
                if (File.Exists(indexFile) && File.Exists(stringFile))
                {
                    var hashedStrings = new IDStringTable();
                    hashedStrings.Read(indexFile, stringFile);
                    foreach (KeyValuePair<ulong, string> hashAndValue in hashedStrings.AsDictionary())
                    {
                        if (!modelIDs.ContainsKey(hashAndValue.Key))
                        {
                            modelIDs.Add(hashAndValue.Key, hashAndValue.Value);
                        }
                    }
                }
            }
        }

        public void WriteToCSV()
        {
            using (TextWriter carOutput = new StreamWriter(File.Create("Cars.csv"), Encoding.UTF8))
            {
                using (CsvWriter carCsv = new CsvWriter(carOutput))
                {
                    carCsv.Configuration.QuoteAllFields = true;
                    carCsv.WriteField("ModelID");
                    carCsv.WriteField("ColourIDs");
                    carCsv.NextRecord();

                    foreach (Car car in Cars.OrderBy(car => car.ModelName))
                    {
                        car.WriteToCSV(carCsv);
                    }
                }
            }

            using (TextWriter colourOutput = new StreamWriter(File.Create("Colours.csv"), Encoding.UTF8))
            {
                using (CsvWriter colourCsv = new CsvWriter(colourOutput))
                {
                    colourCsv.Configuration.QuoteAllFields = true;
                    colourCsv.WriteField("ColourID");
                    colourCsv.WriteField("LatinName");
                    colourCsv.WriteField("JapaneseName");
                    colourCsv.WriteField("ThumbnailColour");
                    colourCsv.NextRecord();

                    foreach (CarColour colour in Colours.Values)
                    {
                        colour.WriteToCSV(colourCsv);
                    }
                }
            }
        }
    }
}