using CsvHelper.Configuration;
using System.IO;

namespace GT2DataSplitter
{
    public class CarCsvDataStructure<TStructure, TMap> : CsvDataStructure<TStructure, TMap> where TMap : ClassMap
    {
        public bool HasCarId { get; set; } = true;

        public CarCsvDataStructure()
        {
            CacheFilename = true;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            string filename = Name;

            if (HasCarId)
            {
                uint carID = data.ReadUInt();
                filename += "\\" + Utils.GetCarName(carID);

                if (!Directory.Exists(filename))
                {
                    Directory.CreateDirectory(filename);
                }
            }

            string number = Directory.GetFiles(filename).Length.ToString();

            for (int i = number.Length; i < 4; i++)
            {
                number = "0" + number;
            }

            return filename + "\\" + number + "0.csv";
        }
    }
}
