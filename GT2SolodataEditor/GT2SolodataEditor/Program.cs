using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;

namespace GT2.SolodataEditor
{
    using CarNameConversion;
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
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

                using (TextWriter output = new StreamWriter(File.Create("Cars.csv"), Encoding.UTF8))
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
            using (FileStream file = new FileStream("new_solodata.dat", FileMode.Create, FileAccess.Write))
            {
                using (TextReader output = new StreamReader(File.OpenRead("Menus.csv"), Encoding.UTF8))
                {
                    using (CsvReader csv = new CsvReader(output))
                    {
                        file.WriteUInt(0);
                        uint rowCount = 0;
                        csv.Read();

                        while (csv.Read())
                        {
                            string key = csv.GetField(0);
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
                
                using (TextReader output = new StreamReader(File.OpenRead("Cars.csv"), Encoding.UTF8))
                {
                    using (CsvReader csv = new CsvReader(output))
                    {
                        long startPosition = file.Position;
                        file.WriteUInt(0);
                        uint rowCount = 0;
                        csv.Read();

                        while (csv.Read())
                        {
                            string key = csv.GetField(0);
                            file.WriteUInt(CarNameConversion.ToCarID(key));
                            string value = csv.GetField(1);
                            file.WriteUInt(ushort.Parse(value, System.Globalization.NumberStyles.HexNumber));
                            rowCount++;
                        }

                        long filePosition = file.Position;
                        file.Position = startPosition;
                        file.WriteUInt(rowCount);
                        file.Position = filePosition;
                    }
                }
            }
        }
    }
}
