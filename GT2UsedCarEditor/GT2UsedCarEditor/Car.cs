using CsvHelper;
using System.IO;

namespace GT2.UsedCarEditor
{
    using CarNameConversion;
    using StreamExtensions;

    class Car
    {
        public string Name { get; set; }
        public uint Price { get; set; }
        public byte PaletteID { get; set; }

        public void Read(Stream stream)
        {
            Name = stream.ReadUInt().ToCarName();
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

        public void ReadCSV(CsvReader csv)
        {
            Name = csv.GetField(0);
            Price = uint.Parse(csv.GetField(1));
            PaletteID = byte.Parse(csv.GetField(2), System.Globalization.NumberStyles.HexNumber);
        }

        public void Write(Stream stream)
        {
            stream.WriteUInt(Name.ToCarID());
            WritePrice(stream);
            stream.WriteByte(PaletteID);
        }

        public void WritePrice(Stream stream)
        {
            byte[] byteArray = new byte[3];
            byteArray[2] = (byte)((Price / 256 / 256) % 256);
            byteArray[1] = (byte)((Price / 256) % 256);
            byteArray[0] = (byte)(Price % 256);
            stream.Write(byteArray, 0, 3);
        }
    }
}
