using System.Globalization;
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
                ColourID = (byte)(file.ReadSingleByte() / 2)
            };

        public void WriteToCSV(CsvWriter csv)
        {
            csv.WriteField(CarID.GetNameString(ID));
            csv.WriteField(Price * 10);
            csv.WriteField(string.Format("{0:X2}", ColourID));
            csv.NextRecord();
        }

        public static Car ReadFromCSV(CsvReader csv) =>
            new Car
            {
                ID = CarID.GetNumericID(csv.GetField(0) ?? ""),
                Price = (ushort)(int.Parse(csv.GetField(1) ?? "") / 10),
                ColourID = byte.Parse(csv.GetField(2) ?? "", NumberStyles.HexNumber)
            };

        public void WriteToFile(Stream file)
        {
            file.WriteUShort(Price);
            file.WriteByte(ID);
            file.WriteByte((byte)(ColourID * 2));
        }
    }
}