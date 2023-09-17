using CsvHelper;
using StreamExtensions;

namespace GT1.UsedCarEditor
{
    public class Car
    {
        public byte ID { get; set; }
        public byte ColourID { get; set; }
        public ushort Price { get; set; }

        public static Car ReadFromFile(Stream file) =>
            new Car
            {
                Price = file.ReadUShort(),
                ID = file.ReadSingleByte(),
                ColourID = file.ReadSingleByte(),
            };

        public void WriteToCSV(CsvWriter csv)
        {
            csv.WriteField(CarID.GetNameString(ID));
            csv.WriteField(Price * 10);
            csv.WriteField(string.Format("{0:X2}", ColourID));
            csv.NextRecord();
        }
    }
}