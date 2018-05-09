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

        public static Car ReadFromFiles(FileSet files)
        {
            Car car = new Car();
            car.CarID = files.JPCarInfo.ReadUInt().ToCarName();
            long indexPosition = files.JPCarInfo.Position;
            ushort index = files.JPCarInfo.ReadUShort();
            byte colourCount = (byte)(((files.JPCarInfo.ReadByte() & 0x3C) >> 2) + 1);

            byte regionBlocking = (byte)files.JPCarInfo.ReadByte();
            car.BlockedInUSA = (regionBlocking & 1) == 1;
            car.BlockedInPALFIGS = (regionBlocking & 2) == 2;
            car.BlockedInPALEnglish = (regionBlocking & 4) == 4;

            long headerPosition = files.JPCarInfo.Position;
            files.JPCarInfo.Position = index;
            
            ReadColoursFromFile(files, car, colourCount);
            
            car.JPName = ReadName(files.JPCarInfo, indexPosition, colourCount);
            car.EUName = ReadName(files.EUCarInfo, indexPosition, colourCount);
            car.USName = ReadName(files.USCarInfo, indexPosition, colourCount);

            files.JPCarInfo.Position = headerPosition;

            return car;
        }

        public static string ReadName(Stream stream, long indexPosition, byte colourCount)
        {
            if (stream == null)
            {
                return null;
            }

            stream.Position = indexPosition;
            ushort index = stream.ReadUShort();
            stream.Position = index + (colourCount * 3);
            byte stringLength = (byte)stream.ReadByte();
            byte[] stringBytes = new byte[stringLength];
            stream.Read(stringBytes);
            return Encoding.ASCII.GetString(stringBytes).TrimEnd('\0').Replace(((char)0x7F).ToString(), "[R]");
        }

        public static void ReadColoursFromFile(FileSet files, Car car, byte colourCount)
        {
            car.Colours = new List<CarColour>();
            long startingPosition = files.JPCarInfo.Position;

            for (int i = 0; i < colourCount; i++)
            {
                CarColour colour = new CarColour();

                files.JPCarInfo.Position = startingPosition + (i * 2);
                colour.ThumbnailColour = files.JPCarInfo.ReadUShort();

                files.JPCarInfo.Position = startingPosition + (colourCount * 2) + i;
                files.JPCarInfo.ReadByte(); // string index - todo

                car.Colours.Add(colour);
            }
        }
    }
}
