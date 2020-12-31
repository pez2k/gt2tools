using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using StreamExtensions;

namespace GT3.CarColorEditor
{
    public class CarColour
    {
        public const int RawSize = sizeof(uint) * 4;

        public uint ColourID { get; set; }
        public string LatinName { get; set; }
        public string JapaneseName { get; set; }
        public uint ThumbnailColour { get; set; }
        public string HexThumbnailColour
        {
            get
            {
                byte[] colourBytes = ThumbnailColour.ToByteArray();
                byte thumbnailR = colourBytes[0];
                byte thumbnailG = colourBytes[1];
                byte thumbnailB = colourBytes[2];
                return $"#{thumbnailR:X2}{thumbnailG:X2}{thumbnailB:X2}";
            }
            set
            {
                byte[] number = uint.Parse(value.Replace("#", ""), NumberStyles.HexNumber).ToByteArray();
                ThumbnailColour = (uint)(number[2] + (number[1] * 256) + (number[0] * 256 * 256));
            }
        }

        public void ReadFromGameFiles(Stream file, StringTable colourNames)
        {
            ColourID = file.ReadUInt();
            LatinName = colourNames.Get((ushort)file.ReadUInt());
            JapaneseName = colourNames.Get((ushort)file.ReadUInt());
            ThumbnailColour = file.ReadUInt();
        }

        public void WriteToCSV(CsvWriter csv)
        {
            csv.NextRecord();
            csv.WriteRecord(this);
        }

        public void WriteToGameFiles(Stream file, StringTable colourNames)
        {
            file.WriteUInt(ColourID);
            file.WriteUInt(colourNames.Add(LatinName));
            file.WriteUInt(colourNames.Add(JapaneseName));
            file.WriteUInt(ThumbnailColour);
        }

        public void WriteNameToGameFiles(StringTable colourNames)
        {
            colourNames.Add(LatinName);
            colourNames.Add(JapaneseName);
        }

        public sealed class CSVMap : ClassMap<CarColour>
        {
            public CSVMap()
            {
                Map(m => m.ColourID);
                Map(m => m.LatinName);
                Map(m => m.JapaneseName);
                Map(m => m.HexThumbnailColour).Name("ThumbnailColour");
            }
        }
    }
}