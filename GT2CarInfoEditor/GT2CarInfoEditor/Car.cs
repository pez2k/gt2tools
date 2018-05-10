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

        public void ReadFromFiles(FileSet files, uint carNumber)
        {
            files.JPCarInfo.Position = (carNumber + 1) * 8;
            CarID = files.JPCarInfo.ReadUInt().ToCarName();
            ushort index = files.JPCarInfo.ReadUShort();
            byte colourCount = GetColourCount((byte)files.JPCarInfo.ReadByte());
            SetRegionBlockingFlags((byte)files.JPCarInfo.ReadByte());
            Colours = ReadColoursFromFile(files, index, carNumber, colourCount);
            JPName = ReadName(files.JPCarInfo, carNumber, colourCount);
            EUName = ReadName(files.EUCarInfo, carNumber, colourCount);
            USName = ReadName(files.USCarInfo, carNumber, colourCount);
        }

        public byte GetColourCount(byte rawData) => (byte)(((rawData & 0x3C) >> 2) + 1);

        public void SetRegionBlockingFlags(byte flags)
        {
            BlockedInUSA = (flags & 1) == 1;
            BlockedInPALFIGS = (flags & 2) == 2;
            BlockedInPALEnglish = (flags & 4) == 4;
        }

        public string ReadName(Stream stream, uint carNumber, byte colourCount)
        {
            if (stream == null)
            {
                return null;
            }

            stream.Position = ((carNumber + 1) * 8) + 4;
            ushort index = stream.ReadUShort();
            stream.Position = index + (colourCount * 3);
            byte stringLength = (byte)stream.ReadByte();
            byte[] stringBytes = new byte[stringLength];
            stream.Read(stringBytes);
            return Encoding.ASCII.GetString(stringBytes).TrimEnd('\0').Replace(((char)0x7F).ToString(), "[R]");
        }

        public List<CarColour> ReadColoursFromFile(FileSet files, ushort index, uint carNumber, byte colourCount)
        {
            var colours = new List<CarColour>();

            for (byte i = 0; i < colourCount; i++)
            {
                CarColour colour = new CarColour();
                colour.ReadFromFiles(files, index, carNumber, colourCount, i);
                colours.Add(colour);
            }

            return colours;
        }
    }
}
