using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GT2.CarInfoEditor
{
    using CarNameConversion;
    using StreamExtensions;

    class Car
    {
        // Region flags guess:
        // bit enabled = blocked
        // bit 1, right to left: USA
        // bit 2, PAL FIGS
        // bit 3, PAL English
        // bit 4, unused / unknown

        public string CarID { get; set; }
        public string JPName { get; set; }
        public string USName { get; set; }
        public string EUName { get; set; }
        public List<CarColour> Colours { get; set; }
        public bool BlockedInUSA { get; set; }
        public bool BlockedInPALFIGS { get; set; }
        public bool BlockedInPALEnglish { get; set; }

        public static Car ReadFromFile(Stream stream)
        {
            Car car = new Car();
            car.CarID = stream.ReadUInt().ToCarName();
            ushort index = stream.ReadUShort();
            byte colourCount = (byte)(((stream.ReadByte() & 0x3C) >> 2) + 1);

            byte regionBlocking = (byte)stream.ReadByte();
            car.BlockedInUSA = (regionBlocking & 1) == 1;
            car.BlockedInPALFIGS = (regionBlocking & 2) == 2;
            car.BlockedInPALEnglish = (regionBlocking & 4) == 4;

            long headerPosition = stream.Position;
            stream.Position = index;
            
            ReadColoursFromFile(stream, car, colourCount);

            byte stringLength = (byte)stream.ReadByte();
            byte[] stringBytes = new byte[stringLength];
            stream.Read(stringBytes);
            car.EUName = Encoding.ASCII.GetString(stringBytes).TrimEnd('\0').Replace(((char)0x7F).ToString(), "[R]");

            stream.Position = headerPosition;
            return car;
        }

        public static void ReadColoursFromFile(Stream stream, Car car, byte colourCount)
        {
            car.Colours = new List<CarColour>();
            long startingPosition = stream.Position;

            for (int i = 0; i < colourCount; i++)
            {
                CarColour colour = new CarColour();

                stream.Position = startingPosition + (i * 2);
                colour.ThumbnailColour = stream.ReadUShort();

                stream.Position = startingPosition + (colourCount * 2) + i;
                stream.ReadByte(); // string index - todo

                car.Colours.Add(colour);
            }
        }
    }
}
