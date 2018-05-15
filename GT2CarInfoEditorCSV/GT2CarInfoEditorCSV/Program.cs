using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Text;

namespace GT2.CarInfoEditorCSV
{
    using CarInfoEditor;

    class Program
    {
        static void Main(string[] args)
        {
            CarList list = new CarList();

            using (FileSet files = FileSet.OpenRead())
            {
                list.ReadFromFiles();
            }

            using (TextWriter output = new StreamWriter(File.Create("test.csv"), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(output))
                {
                    using (TextWriter colourOutput = new StreamWriter(File.Create("test2.csv"), Encoding.UTF8))
                    {
                        using (CsvWriter colourCsv = new CsvWriter(colourOutput))
                        {
                            csv.Configuration.RegisterClassMap<CarCSVMap>();
                            csv.Configuration.QuoteAllFields = true;
                            csv.WriteHeader<Car>();

                            colourCsv.Configuration.RegisterClassMap<CarColourCSVMap>();
                            colourCsv.Configuration.QuoteAllFields = true;
                            colourCsv.WriteField("CarName");
                            colourCsv.WriteHeader<CarColour>();

                            foreach (Car car in list.Cars)
                            {
                                csv.NextRecord();
                                csv.WriteRecord(car);
                                
                                foreach (CarColour colour in car.Colours)
                                {
                                    colourCsv.NextRecord();
                                    colourCsv.WriteField(car.CarName);
                                    colourCsv.WriteRecord(colour);
                                }
                            }
                        }
                    }
                }
            }
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

    public sealed class CarColourCSVMap : ClassMap<CarColour>
    {
        public CarColourCSVMap()
        {
            Map(m => m.HexColour);
            Map(m => m.PaletteID);
            Map(m => m.JapaneseName);
            Map(m => m.LatinName);
        }
    }
}
