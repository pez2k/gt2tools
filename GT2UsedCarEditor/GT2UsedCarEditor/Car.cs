using CsvHelper;
using System.IO;

namespace GT2UsedCarEditor
{
    class Car
    {
        public string Name { get; set; }
        public uint Price { get; set; }
        public byte PaletteID { get; set; }

        public void Read(Stream stream)
        {
            Name = Utils.GetCarName(stream.ReadUInt());
            Price = ReadPrice(stream);
            PaletteID = (byte)stream.ReadByte();
        }

        public uint ReadPrice(Stream stream)
        {
            byte[] rawValue = new byte[3];
            stream.Read(rawValue, 0, 3);
            return (uint)(rawValue[2] * 256 * 256 + rawValue[1] * 256 + rawValue[0]);
        }

        public void WriteCSV(CsvWriter csv)
        {
            csv.WriteField(Name);
            csv.WriteField(Price);
            csv.WriteField(string.Format("{0:X2}", PaletteID));
            csv.NextRecord();
        }
    }
}
