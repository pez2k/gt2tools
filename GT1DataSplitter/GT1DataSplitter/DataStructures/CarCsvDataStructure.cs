using System.IO;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using StreamExtensions;

    public abstract class CarCsvDataStructure<TStructure, TMap> : CsvDataStructure<TStructure, TMap> where TMap : ClassMap
    {
        protected bool hasCarId = true;

        protected CarCsvDataStructure() => cacheFilename = true;

        protected override string CreateOutputFilename()
        {
            string filename = Name;

            /*if (hasCarId)
            {
                uint carID = rawData.ReadUInt();
                filename += "\\" + carID.ToCarName();
                if (!Directory.Exists(filename))
                {
                    Directory.CreateDirectory(filename);
                }
            }*/

            string number = Directory.GetFiles(filename).Length.ToString();
            for (int i = number.Length; i < 4; i++)
            {
                number = "0" + number;
            }
            return filename + "\\" + number + "0.csv";
        }

        public string CreateOutputFilename(uint carId, byte stage)
        {
            string filename = Name;
            /*string filename = Name + "\\" + carId.ToCarName();
            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }*/
            string number = Directory.GetFiles(filename).Length.ToString();
            return filename + "\\" + number + "_stage" + stage.ToString() + ".csv";
        }
    }
}