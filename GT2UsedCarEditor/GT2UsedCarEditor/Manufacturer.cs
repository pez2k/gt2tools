using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GT2UsedCarEditor
{
    class Manufacturer
    {
        public string Name { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
        
        public void Read(Stream stream, uint startPosition, ushort carCount)
        {
            for (int i = 0; i < carCount; i++)
            {
                stream.Position = (i * 8) + startPosition;
                var car = new Car();
                car.Read(stream);
                Cars.Add(car);
            }
        }

        public void WriteCSV(string directory)
        {
            if (Cars.Count == 0)
            {
                return;
            }

            using (TextWriter file = new StreamWriter(File.Create(directory + "\\" + Name + ".csv"), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(file, new Configuration() { QuoteAllFields = true }))
                {
                    csv.WriteField("Car");
                    csv.WriteField("Price");
                    csv.WriteField("Colour");
                    csv.NextRecord();

                    foreach (Car car in Cars)
                    {
                        car.WriteCSV(csv);
                    }
                }
            }
        }

        public void ReadCSV(string filename)
        {
            using (TextReader file = new StreamReader(filename, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(file, new Configuration() { QuoteAllFields = true }))
                {
                    csv.Read();
                    
                    while(csv.Read())
                    {
                        var car = new Car();
                        car.ReadCSV(csv);
                        Cars.Add(car);
                    }
                }
            }
        }

        public uint Write(Stream stream, uint indexPosition, uint blockStart, uint dataPosition)
        {
            stream.Position = indexPosition;
            stream.WriteUShort((ushort)(dataPosition - blockStart));
            stream.WriteUShort((ushort)Cars.Count);
            stream.Position = dataPosition;

            foreach (Car car in Cars)
            {
                car.Write(stream);
            }

            return (uint)stream.Position;
        }
    }
}
