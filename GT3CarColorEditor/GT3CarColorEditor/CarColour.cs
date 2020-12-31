﻿using System.IO;
using CsvHelper;
using StreamExtensions;

namespace GT3.CarColorEditor
{
    public class CarColour
    {
        public uint ColourID { get; set; }
        public string LatinName { get; set; }
        public string JapaneseName { get; set; }
        public uint ThumbnailColour { get; set; }

        public void ReadFromGameFiles(Stream file, StringTable colourNames)
        {
            ColourID = file.ReadUInt();
            LatinName = colourNames.Get((ushort)file.ReadUInt());
            JapaneseName = colourNames.Get((ushort)file.ReadUInt());
            ThumbnailColour = file.ReadUInt();
        }

        public void WriteToCSV(CsvWriter colourCsv)
        {
            byte[] colourBytes = ThumbnailColour.ToByteArray();
            byte thumbnailR = colourBytes[0];
            byte thumbnailG = colourBytes[1];
            byte thumbnailB = colourBytes[2];

            colourCsv.WriteField(ColourID);
            colourCsv.WriteField(LatinName);
            colourCsv.WriteField(JapaneseName);
            colourCsv.WriteField($"#{thumbnailR:X2}{thumbnailG:X2}{thumbnailB:X2}");
            colourCsv.NextRecord();
        }
    }
}