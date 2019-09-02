using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GT2.CarInfoEditor
{
    using CarNameConversion;
    using StreamExtensions;

    public class Car
    {
        public string CarName { get; set; }
        public string JPName { get; set; }
        public string USName { get; set; }
        public string EUName { get; set; }
        public List<CarColour> Colours { get; set; }
        public bool BlockedInJapan { get; set; }
        public bool BlockedInUSA { get; set; }
        public bool BlockedInPALFIGS { get; set; }
        public bool BlockedInPALEnglish { get; set; }

        [Flags]
        protected enum RegionBlockingFlags
        {
            None = 0,
            Japan = 1,
            USA = 2,
            PALFIGS = 4,
            PALEnglish = 8
        }

        public void ReadFromFiles(FileSet files, uint carNumber)
        {
            files.JPCarInfo.Position = (carNumber + 1) * 8;
            CarName = files.JPCarInfo.ReadUInt().ToCarName();
            ushort index = files.JPCarInfo.ReadUShort();
            ushort miscData = files.JPCarInfo.ReadUShort();
            byte colourCount = SetColourCount(miscData);
            SetRegionBlockingFlags(miscData);
            Colours = ReadColoursFromFiles(files, index, carNumber, colourCount);
            JPName = ReadName(files.JPCarInfo, carNumber, colourCount);
            EUName = ReadName(files.EUCarInfo, carNumber, colourCount);
            USName = ReadName(files.USCarInfo, carNumber, colourCount);
        }

        public byte SetColourCount(ushort rawData) => (byte)(((rawData & 0x3C) >> 2) + 1);

        protected void SetRegionBlockingFlags(ushort rawData)
        {
            RegionBlockingFlags flags = (RegionBlockingFlags)((rawData & 0x780) >> 7);
            BlockedInJapan = flags.HasFlag(RegionBlockingFlags.Japan);
            BlockedInUSA = flags.HasFlag(RegionBlockingFlags.USA);
            BlockedInPALFIGS = flags.HasFlag(RegionBlockingFlags.PALFIGS);
            BlockedInPALEnglish = flags.HasFlag(RegionBlockingFlags.PALEnglish);
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
            return Encoding.Default.GetString(stringBytes).TrimEnd('\0').Replace(((char)0x7F).ToString(), "[R]");
        }

        public List<CarColour> ReadColoursFromFiles(FileSet files, ushort index, uint carNumber, byte colourCount)
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

        public void WriteToFiles(FileSet files, int carNumber)
        {
            List<long> indexes = new List<long>(3);

            foreach (Stream file in files.CarInfoFiles)
            {
                file.Position = (carNumber + 1) * 8;
                file.WriteUInt(CarName.ToCarID());
                long index = file.Length;
                file.WriteUShort((ushort)index);
                indexes.Add(index);
                file.WriteUShort(GetColourCountAndRegionBlockingFlags());
            }
            WriteColoursToFiles(files, indexes, carNumber);
            WriteName(files.JPCarInfo, JPName, indexes[0], Colours.Count);
            WriteName(files.USCarInfo, USName, indexes[1], Colours.Count);
            WriteName(files.EUCarInfo, EUName, indexes[2], Colours.Count);
        }

        public ushort GetColourCountAndRegionBlockingFlags()
        {
            RegionBlockingFlags flags = RegionBlockingFlags.None;

            if (BlockedInJapan)
            {
                flags |= RegionBlockingFlags.Japan;
            }
            if (BlockedInUSA)
            {
                flags |= RegionBlockingFlags.USA;
            }
            if (BlockedInPALFIGS)
            {
                flags |= RegionBlockingFlags.PALFIGS;
            }
            if (BlockedInPALEnglish)
            {
                flags |= RegionBlockingFlags.PALEnglish;
            }

            return (ushort)(((((ushort)flags) << 7) & 0x780) | ((Colours.Count - 1) << 2) & 0x3C);
        }

        public void WriteName(Stream stream, string name, long index, int colourCount)
        {
            if (stream == null)
            {
                return;
            }
            
            stream.Position = index + (colourCount * 3);
            name = name.Replace("[R]", ((char)0x7F).ToString()) + "\0";
            stream.WriteByte((byte)(name.Length - 1));
            stream.Write(name.ToByteArray());
            if (stream.Length % 2 != 0)
            {
                stream.WriteByte(0);
            }
        }

        public void WriteColoursToFiles(FileSet files, List<long> carInfoIndexes, int carNumber)
        {
            long carColourIndex = files.CarColours.Length;
            files.CarColours.Position = (carNumber * 2) + 8;
            files.CarColours.WriteUShort((ushort)carColourIndex);

            for (byte i = 0; i < Colours.Count; i++)
            {
                Colours[i].WriteToFiles(files, carInfoIndexes, carColourIndex, Colours.Count, i);
            }
        }
    }
}
