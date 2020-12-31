using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using StreamExtensions;

namespace GT3.CarColorEditor
{
    public class Car
    {
        public ulong ModelNameHash { get; set; }
        public string ModelName { get; set; }
        public List<uint> ColourIDs { get; set; } = new List<uint>();

        public void ReadFromGameFiles(Stream file, uint carToColoursMapIndex, Dictionary<ulong, string> modelIDs, SortedDictionary<uint, CarColour> colours)
        {
            ModelNameHash = file.ReadULong();
            uint carColourCount = file.ReadUInt();
            uint colourMapOffset = file.ReadUInt();

            long currentOffset = file.Position;
            file.Position = carToColoursMapIndex + colourMapOffset;

            for (uint i = 0; i < carColourCount; i++)
            {
                uint colourID = file.ReadUInt();
                if (!colours.ContainsKey(colourID))
                {
                    throw new Exception("Invalid colour ID.");
                }
                ColourIDs.Add(colourID);
            }

            file.Position = currentOffset;
            ModelName = modelIDs.TryGetValue(ModelNameHash, out string carIDString) ? carIDString : $"0x{ModelNameHash:X16}";
        }


        public void WriteToCSV(CsvWriter carCsv)
        {
            carCsv.WriteField(ModelName);
            carCsv.WriteField(string.Join(",", ColourIDs.Select(id => $"{id}")));
            carCsv.NextRecord();
        }
    }
}