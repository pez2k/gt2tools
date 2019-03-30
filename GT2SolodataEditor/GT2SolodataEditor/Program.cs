using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using ICSharpCode.SharpZipLib.GZip;

namespace GT2.SolodataEditor
{
    using CarNameConversion;
    using StreamExtensions;

    class Program
    {
        static Dictionary<string, ushort> Cars = new Dictionary<string, ushort>();

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                Dump();
            }
            else
            {
                Build();
            }
        }

        static void Dump()
        {
            var menus = new Dictionary<string, ushort>();
            var cars = new Dictionary<string, uint>();

            using (FileStream file = new FileStream("solodata.dat", FileMode.Open, FileAccess.Read))
            {
                uint menuCount = file.ReadUInt();
                for (int i = 0; i < menuCount; i++)
                {
                    menus.Add($"Menu{i}", file.ReadUShort());
                }
                uint carCount = file.ReadUInt();
                for (int i = 0; i < carCount; i++)
                {
                    cars.Add(file.ReadUInt().ToCarName(), file.ReadUInt());
                }

                using (TextWriter output = new StreamWriter(File.Create("Menus.csv"), Encoding.UTF8))
                {
                    using (CsvWriter csv = new CsvWriter(output))
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.WriteField("Menu");
                        csv.WriteField("ID");
                        csv.NextRecord();

                        foreach (var item in menus)
                        {
                            csv.WriteField(item.Key);
                            csv.WriteField($"{item.Value:X4}");
                            csv.NextRecord();
                        }
                    }
                }

                if (!Directory.Exists("Cars"))
                {
                    Directory.CreateDirectory("Cars");
                }

                using (TextWriter output = new StreamWriter(File.Create("Cars\\Cars.csv"), Encoding.UTF8))
                {
                    using (CsvWriter csv = new CsvWriter(output))
                    {
                        csv.Configuration.QuoteAllFields = true;
                        csv.WriteField("Car");
                        csv.WriteField("Description");
                        csv.NextRecord();

                        foreach (var item in cars)
                        {
                            csv.WriteField(item.Key);
                            csv.WriteField($"{item.Value:X4}");
                            csv.NextRecord();
                        }
                    }
                }
            }
        }

        static void Build()
        {
            using (FileStream file = new FileStream("new_solodata.dat", FileMode.Create, FileAccess.ReadWrite))
            {
                using (TextReader input = new StreamReader(File.OpenRead("Menus.csv"), Encoding.UTF8))
                {
                    using (CsvReader csv = new CsvReader(input))
                    {
                        file.WriteUInt(0);
                        uint rowCount = 0;
                        csv.Read();

                        while (csv.Read())
                        {
                            string value = csv.GetField(1);
                            file.WriteUShort(ushort.Parse(value, System.Globalization.NumberStyles.HexNumber));
                            rowCount++;
                        }

                        long filePosition = file.Position;
                        file.Position = 0;
                        file.WriteUInt(rowCount);
                        file.Position = filePosition;
                    }
                }

                ImportCSV("Cars\\Cars.csv");

                var filenames = Directory.EnumerateFiles("Cars", "*.csv").Where(csv => Path.GetFileName(csv) != "Cars.csv");

                foreach (string filename in filenames)
                {
                    ImportCSV(filename);
                }

                long startPosition = file.Position;
                file.WriteUInt(0);

                foreach (var car in Cars)
                {
                    file.WriteUInt(CarNameConversion.ToCarID(car.Key));
                    file.WriteUInt(car.Value);
                }
                
                file.Position = startPosition;
                file.WriteUInt((uint)Cars.Count);

                file.Position = 0;
                using (var gzip = new FileStream("new_solodata.dat.gz", FileMode.Create, FileAccess.Write))
                {
                    using (var compression = new GZipOutputStream(gzip))
                    {
                        compression.SetLevel(8);
                        compression.IsStreamOwner = false;
                        file.CopyTo(compression);
                    }
                }
            }
        }

        static void ImportCSV(string filename)
        {
            using (TextReader input = new StreamReader(File.OpenRead(filename), Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(input))
                {
                    csv.Read();
                    while (csv.Read())
                    {
                        string key = csv.GetField(0);
                        string value = csv.GetField(1);
                        Cars.Remove(key);
                        if (!string.IsNullOrEmpty(value))
                        {
                            Cars.Add(key, ushort.Parse(value, System.Globalization.NumberStyles.HexNumber));
                        }
                    }
                }
            }
        }
    }
}
