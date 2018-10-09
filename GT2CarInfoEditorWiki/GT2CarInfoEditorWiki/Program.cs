using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GT2.CarInfoEditorCSV
{
    using CarInfoEditor;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            string file = Path.GetFileName(args[0]);

            if (file.StartsWith(".carinfo"))
            {
                Dump();
            }
        }

        static void Dump()
        {
            CarList list = new CarList();
            list.ReadFromFiles();

            using (TextWriter output = new StreamWriter(File.Create("CarColours.txt"), Encoding.UTF8))
            {
                foreach (Car car in list.Cars)
                {
                    output.WriteLine($"{car.CarName} ({car.JPName} / {car.USName} / {car.EUName})");
                    output.WriteLine();
                    output.WriteLine("==Colors==");
                    if (car.Colours.Count > 1)
                    {
                        output.Write($"There are {GetNumber(car.Colours.Count)} colors");
                    }
                    else
                    {
                        output.Write($"There is only one color");
                    }
                    output.Write(" available for this vehicle");

                    bool hasColourNames = false;
                    foreach (CarColour colour in car.Colours)
                    {
                        if (!string.IsNullOrWhiteSpace(colour.LatinName))
                        {
                            hasColourNames = true;
                            break;
                        }
                    }

                    if (!hasColourNames)
                    {
                        if (car.Colours.Count > 1)
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
                    
                    foreach (CarColour colour in car.Colours)
                    {
                        CarColourWithName colourWithName = new CarColourWithName
                        {
                            CarName = car.CarName,
                            ThumbnailColour = colour.ThumbnailColour,
                            PaletteID = colour.PaletteID,
                            JapaneseName = colour.JapaneseName,
                            LatinName = colour.LatinName
                        };

                        output.WriteLine($"*{{{{ColorSquare|{colour.HexColour}}}}}{(string.IsNullOrWhiteSpace(colour.LatinName) ? "" : " " + colour.LatinName)}");
                    }

                    output.WriteLine();
                    output.WriteLine();
                }
            }
        }

        static string GetNumber(long number) {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                case 10:
                    return "ten";
                case 11:
                    return "eleven";
                case 12:
                    return "twelve";
                case 13:
                    return "thirteen";
                case 14:
                    return "fourteen";
                case 15:
                    return "fifteen";
                case 16:
                    return "sixteen";
            }
            throw new System.Exception();
        }

        static void Load()
        {
            CarList list = new CarList();
            list.Cars = new List<Car>();

            using (TextReader input = new StreamReader("Cars.csv", Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(input))
                {
                    using (TextReader colourInput = new StreamReader("Colours.csv", Encoding.UTF8))
                    {
                        using (CsvReader colourCsv = new CsvReader(colourInput))
                        {
                            csv.Configuration.RegisterClassMap<CarCSVMap>();
                            colourCsv.Configuration.RegisterClassMap<CarColourCSVMap>();

                            while (csv.Read())
                            {
                                Car newCar = csv.GetRecord<Car>();
                                newCar.Colours = new List<CarColour>();
                                list.Cars.Add(newCar);
                            }
                            
                            while (colourCsv.Read())
                            {
                                CarColourWithName newColourWithName = colourCsv.GetRecord<CarColourWithName>();
                                string carName = newColourWithName.CarName;
                                CarColour newColour = new CarColour
                                {
                                    ThumbnailColour = newColourWithName.ThumbnailColour,
                                    PaletteID = newColourWithName.PaletteID,
                                    JapaneseName = newColourWithName.JapaneseName,
                                    LatinName = newColourWithName.LatinName
                                };
                                list.Cars.Find(car => car.CarName == carName).Colours.Add(newColour);
                            }
                        }
                    }
                }
            }
            
            list.SaveToFiles();
        }
    }

    public sealed class CarCSVMap : ClassMap<Car>
    {
        public CarCSVMap()
        {
            Map(m => m.CarName);
            Map(m => m.JPName);
            Map(m => m.USName);
            Map(m => m.EUName);
            Map(m => m.BlockedInJapan);
            Map(m => m.BlockedInUSA);
            Map(m => m.BlockedInPALFIGS);
            Map(m => m.BlockedInPALEnglish);
        }
    }

    public class CarColourWithName : CarColour
    {
        public string CarName { get; set; }
    }

    public sealed class CarColourCSVMap : ClassMap<CarColourWithName>
    {
        public CarColourCSVMap()
        {
            Map(m => m.CarName);
            Map(m => m.HexColour);
            Map(m => m.PaletteID).TypeConverter<HexConverter>();
            Map(m => m.JapaneseName);
            Map(m => m.LatinName);
        }
    }

    public sealed class HexConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return byte.Parse(text, NumberStyles.HexNumber);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return string.Format("{0:X2}", (byte)value);
        }
    }
}
