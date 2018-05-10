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
            files.JPCarInfo.ReadByte(); // string index - todo
        }
    }
}
