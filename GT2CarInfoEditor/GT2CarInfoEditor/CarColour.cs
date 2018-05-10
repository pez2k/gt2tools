using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GT2.CarInfoEditor
{
    using StreamExtensions;

    class CarColour
    {
        public string LatinName { get; set; }
        public string JapaneseName { get; set; }
        public ushort ThumbnailColour { get; set; }
        public byte PaletteID { get; set; }

        public string HexColour
        {
            get
            {
                int R = ThumbnailColour & 0x1F;
                int G = (ThumbnailColour >> 5) & 0x1F;
                int B = (ThumbnailColour >> 10) & 0x1F;
                return $"#{R * 8:X2}{G * 8:X2}{B * 8:X2}";
            }
        }

        public void ReadFromFiles(FileSet files, ushort index, uint carNumber, byte colourCount, byte colourNumber)
        {
            files.JPCarInfo.Position = index + (colourNumber * 2);
            ThumbnailColour = files.JPCarInfo.ReadUShort();

            files.JPCarInfo.Position = index + (colourCount * 2) + colourNumber;
            PaletteID = (byte)files.JPCarInfo.ReadByte();

            ReadName(files, carNumber, colourNumber);
        }
        
        public void ReadName(FileSet files, uint carNumber, byte colourNumber)
        {
            files.CarColours.Position = (carNumber * 2) + 8;
            ushort index = files.CarColours.ReadUShort();
            files.CarColours.Position = index + (colourNumber * 2);
            ushort stringNumber = files.CarColours.ReadUShort();

            LatinName = ReadName(files.CCLatin, stringNumber, false);
            JapaneseName = ReadName(files.CCJapanese, stringNumber, true);
        }

        public string ReadName(Stream stream, ushort stringNumber, bool isUnicode)
        {
            stream.Position = stringNumber * 2;
            ushort index = stream.ReadUShort();
            stream.Position = index;

            List<byte> stringBytes = new List<byte>();

            byte currentByte = 0x00;

            do
            {
                currentByte = (byte)stream.ReadByte();
                if (currentByte != 0x00)
                {
                    stringBytes.Add(currentByte);
                }
            }
            while (currentByte != 0x00);
            
            return (isUnicode ? Encoding.Unicode : Encoding.ASCII).GetString(stringBytes.ToArray()).TrimEnd('\0');
        }
    }
}
