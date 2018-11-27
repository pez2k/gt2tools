using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace GT2.SolodataEditor
{
    using CarNameConversion;
    using CsvHelper.Configuration;
    using StreamExtensions;

    class Program
    {
        static void Main(string[] args)
        {
            Dump();
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
    }
}
