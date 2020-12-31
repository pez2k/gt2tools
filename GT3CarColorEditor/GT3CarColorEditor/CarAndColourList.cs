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
            [HashGenerator.GenerateHash("ma0121")] = "ma0121",
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
            var suffixes = new string[] { "", "_eu", "_us", "_gtc", "_gtc_eu", "_gtc_kr", "_gtc_tw", "_gtc_us" };
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
                using (CsvWriter csv = new CsvWriter(carOutput))
                {
                    csv.Configuration.RegisterClassMap<Car.CSVMap>();
                    csv.Configuration.QuoteAllFields = true;
                    csv.WriteHeader<Car>();

                    foreach (Car car in Cars.OrderBy(car => car.ModelName))
                    {
                        car.WriteToCSV(csv);
                    }
                }
            }

            using (TextWriter colourOutput = new StreamWriter(File.Create("Colours.csv"), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(colourOutput))
                {
                    csv.Configuration.RegisterClassMap<CarColour.CSVMap>();
                    csv.Configuration.QuoteAllFields = true;
                    csv.WriteHeader<CarColour>();

                    foreach (CarColour colour in Colours.Values)
                    {
                        colour.WriteToCSV(csv);
                    }
                }
            }
        }

        public void WriteToWikiText()
        {
            using (TextWriter output = new StreamWriter(File.Create("CarColours.txt"), Encoding.UTF8))
            {
                foreach (Car car in Cars.OrderBy(car => car.ModelName))
                {
                    output.WriteLine(car.ModelName);
                    output.WriteLine();
                    output.WriteLine("==Colors==");
                    List<CarColour> carColours = car.ColourIDs.Select(id => Colours.TryGetValue(id, out CarColour colour) ? colour : throw new Exception("Missing colour.")).ToList();
                    if (carColours.Count > 1)
                    {
                        output.Write($"There are {GetNumber(car.ColourIDs.Length)} colors");
                    }
                    else
                    {
                        output.Write($"There is only one color");
                    }
                    output.Write(" available for this vehicle");

                    bool hasColourNames = false;
                    foreach (CarColour colour in carColours)
                    {
                        if (colour.LatinName != "-")
                        {
                            hasColourNames = true;
                            break;
                        }
                    }

                    if (!hasColourNames)
                    {
                        if (carColours.Count > 1)
                        {
                            output.Write($", they are");
                        }
                        else
                        {
                            output.Write($", it is");
                        }
                        output.Write($" unnamed in-game");
                    }

                    output.WriteLine($":");

                    foreach (CarColour colour in carColours)
                    {
                        output.WriteLine($"*{{{{ColorSquare|{colour.HexThumbnailColour}}}}}" +
                                         $"{(string.IsNullOrWhiteSpace(colour.LatinName) ? "" : " " + (colour.LatinName == colour.JapaneseName ? colour.LatinName : $"{colour.LatinName} ({colour.JapaneseName})"))}");
                    }

                    output.WriteLine();
                    output.WriteLine();
                }
            }
        }

        private string GetNumber(long number)
        {
            Dictionary<long, string> numbers = new Dictionary<long, string>
            {
                [1] = "one",
                [2] = "two",
                [3] = "three",
                [4] = "four",
                [5] = "five",
                [6] = "six",
                [7] = "seven",
                [8] = "eight",
                [9] = "nine",
                [10] = "ten",
                [11] = "eleven",
                [12] = "twelve",
                [13] = "thirteen",
                [14] = "fourteen",
                [15] = "fifteen",
                [16] = "sixteen",
                [17] = "seventeen",
                [18] = "eighteen",
                [19] = "nineteen",
                [20] = "twenty",
                [21] = "twenty-one",
                [22] = "twenty-two",
                [23] = "twenty-three",
                [24] = "twenty-four",
                [25] = "twenty-five",
                [26] = "twenty-six",
                [27] = "twenty-seven"
            };
            return numbers.TryGetValue(number, out string name) ? name : throw new Exception("Number not found");
        }

        public void ReadFromCSV()
        {
            ReadColours("Colours.csv");
            foreach (string csvPath in Directory.EnumerateFiles(".\\", "Colours_*.csv"))
            {
                ReadColours(csvPath);
            }

            ReadCars("Cars.csv");
            foreach (string csvPath in Directory.EnumerateFiles(".\\", "Cars_*.csv"))
            {
                ReadCars(csvPath);
            }
        }

        private void ReadColours(string csvPath)
        {
            using (TextReader file = new StreamReader(csvPath, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(file))
                {
                    csv.Configuration.RegisterClassMap<CarColour.CSVMap>();

                    while (csv.Read())
                    {
                        CarColour newColour = csv.GetRecord<CarColour>();
                        Colours.Remove(newColour.ColourID);
                        if (newColour.JapaneseName != "Delete")
                        {
                            Colours.Add(newColour.ColourID, newColour);
                        }
                    }
                }
            }
        }

        private void ReadCars(string csvPath)
        {
            using (TextReader file = new StreamReader(csvPath, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(file))
                {
                    csv.Configuration.RegisterClassMap<Car.CSVMap>();

                    while (csv.Read())
                    {
                        Car newCar = csv.GetRecord<Car>();
                        if (newCar.ColourIDs.Any(id => !Colours.ContainsKey(id)))
                        {
                            throw new Exception($"Car {newCar.ModelName} uses invalid colour ID.");
                        }
                        Cars.Remove(Cars.Where(car => car.ModelName == newCar.ModelName).SingleOrDefault());
                        Cars.Add(newCar);
                    }
                    Cars = Cars.OrderBy(car => car.ModelNameHash).ToList();
                }
            }
        }

        public void WriteToGameFiles()
        {
            using (var file = new FileStream("new_carcolor.db", FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("GT2K");
                file.WriteUInt(0);
                file.WriteUInt((uint)Cars.Count);

                uint colourMapOffset = (uint)(file.Position + (sizeof(uint) * 3) + (Cars.Count * Car.RawSize));
                file.WriteUInt(colourMapOffset);

                uint colourMapSize = (uint)((Cars.Sum(car => car.ColourIDs.Length) + 1) * sizeof(uint));
                uint coloursOffset = colourMapOffset + colourMapSize;
                file.WriteUInt(coloursOffset);

                uint coloursSize = (uint)((Colours.Values.Count * CarColour.RawSize) + sizeof(uint));
                file.WriteUInt(coloursOffset + coloursSize); // total file size
                long carListOffset = file.Position;

                file.Position = colourMapOffset;
                Car[] alphabeticCars = Cars.OrderBy(car => car.ModelName).ToArray(); // colour names in string table and car to colour ID map are sorted by model name
                uint[] carColours = alphabeticCars.SelectMany(car => car.ColourIDs).ToArray();
                file.WriteUInt((uint)carColours.Length);
                foreach (Car car in alphabeticCars)
                {
                    car.WriteColourIDsToGameFiles(file, colourMapOffset);
                }

                file.Position = carListOffset;
                foreach (Car car in Cars)
                {
                    car.WriteToGameFiles(file);
                }

                file.Position = coloursOffset;
                var colourNames = new StringTable();
                foreach (uint colourID in carColours)
                {
                    if (!Colours.TryGetValue(colourID, out CarColour colour))
                    {
                        throw new Exception($"Invalid colour ID {colourID}.");
                    }
                    colour.WriteNameToGameFiles(colourNames);
                }
                file.WriteUInt((uint)Colours.Count);
                foreach (CarColour colour in Colours.Values)
                {
                    colour.WriteToGameFiles(file, colourNames);
                }
                colourNames.Write("new_carcolor.sdb", true);
            }
        }
    }
}