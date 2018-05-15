using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GT2.CarInfoEditor
{
    using StreamExtensions;

    public class CarColour
    {
        public static List<string> CachedLatinNames { get; set; } = new List<string>();
        public static List<string> CachedJapaneseNames { get; set; } = new List<string>();

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
            set
            {
                value = value.Replace("#", "");
                int number = int.Parse(value, NumberStyles.HexNumber);

                int R = (((number & 0xFF0000) >> 16) / 8) & 0x1F;
                int G = ((((number & 0x00FF00) >> 8) / 8) & 0x1F) << 5;
                int B = ((((number & 0x0000FF)) / 8) & 0x1F) << 10;

                ThumbnailColour = (ushort)(R + G + B);
            }
        }

        public static void ClearCache()
        {
            CachedLatinNames = new List<string>();
            CachedJapaneseNames = new List<string>();
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

        public string ReadName(Stream file, ushort stringNumber, bool isUnicode)
        {
            List<string> cache = isUnicode ? CachedJapaneseNames : CachedLatinNames;

            if (cache.Count > stringNumber)
            {
                return cache[stringNumber];
            }

            file.Position = stringNumber * 2;
            ushort index = file.ReadUShort();
            ushort nextIndex = file.ReadUShort();

            if (nextIndex < index)
            {
                nextIndex = (ushort)file.Length;
            }

            ushort stringLength = (ushort)(nextIndex - index);
            file.Position = index;
            
            byte[] stringBytes = new byte[stringLength];
            file.Read(stringBytes);

            string value = (isUnicode ? Encoding.Unicode : Encoding.Default).GetString(stringBytes).TrimEnd('\0');
            cache.Insert(stringNumber, value);
            return value;
        }

        public void WriteToFiles(FileSet files, List<long> carInfoIndexes, long carColourIndex, int colourCount, byte colourNumber)
        {
            int i = 0;
            foreach (Stream file in files.CarInfoFiles)
            {
                file.Position = carInfoIndexes[i] + (colourNumber * 2);
                file.WriteUShort(ThumbnailColour);

                file.Position = carInfoIndexes[i] + (colourCount * 2) + colourNumber;
                file.WriteByte(PaletteID);
                i++;
            }


            WriteName(files, carColourIndex, colourNumber);
        }

        public void WriteName(FileSet files, long index, byte colourNumber)
        {
            files.CarColours.Position = index + (colourNumber * 2);
            files.CarColours.WriteUShort(CacheNames());
        }

        public ushort CacheNames()
        {
            if (CachedJapaneseNames.Contains(JapaneseName) && CachedLatinNames.Contains(LatinName))
            {
                List<int> JapaneseIndexes = new List<int>();
                int index = -1;

                do
                {
                    index = CachedJapaneseNames.IndexOf(JapaneseName, index + 1);
                    if (index > -1)
                    {
                        JapaneseIndexes.Add(index);
                    }
                }
                while (index > -1);

                index = -1;
                do
                {
                    index = CachedLatinNames.IndexOf(LatinName, index + 1);
                    if (JapaneseIndexes.Contains(index))
                    {
                        return (ushort)index;
                    }
                }
                while (index > -1);
            }

            CachedJapaneseNames.Add(JapaneseName);
            CachedLatinNames.Add(LatinName);
            return (ushort)(CachedJapaneseNames.Count - 1);
        }

        public static void WriteCachedNames(FileSet files)
        {
            WriteCachedNames(files.CCJapanese, CachedJapaneseNames, true);
            WriteCachedNames(files.CCLatin, CachedLatinNames, false);
        }

        private static void WriteCachedNames(Stream file, List<string> cache, bool isUnicode)
        {
            file.SetLength(cache.Count * 2);

            for (int i = 0; i < cache.Count; i++)
            {
                long index = file.Length;
                file.Position = i * 2;
                file.WriteUShort((ushort)index);
                file.Position = index;
                string name = cache[i] + "\0";
                file.Write((isUnicode ? Encoding.Unicode : Encoding.Default).GetBytes(name.ToCharArray()));
            }
        }
    }
}
