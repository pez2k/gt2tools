using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using StreamExtensions;

namespace GT3.CarColorEditor
{
    using CsvHelper.Configuration;
    using HashGenerator;

    public class Car
    {
        private string modelName;
        private uint colourMapOffset;

        public const int RawSize = sizeof(ulong) + (sizeof(uint) * 2);

        public ulong ModelNameHash { get; set; }
        public string ModelName
        {
            get => modelName;
            set
            {
                ModelNameHash = value.StartsWith("0x") ? ulong.Parse(value, NumberStyles.HexNumber) : HashGenerator.GenerateHash(value);
                modelName = value;
            }
        }
        public uint[] ColourIDs { get; set; }

        public void ReadFromGameFiles(Stream file, uint carToColoursMapIndex, Dictionary<ulong, string> modelIDs, SortedDictionary<uint, CarColour> colours)
        {
            ModelNameHash = file.ReadULong();
            uint carColourCount = file.ReadUInt();
            uint colourMapOffset = file.ReadUInt();

            long currentOffset = file.Position;
            file.Position = carToColoursMapIndex + colourMapOffset;

            ColourIDs = new uint[carColourCount];
            for (uint i = 0; i < carColourCount; i++)
            {
                uint colourID = file.ReadUInt();
                if (!colours.ContainsKey(colourID))
                {
                    throw new Exception("Invalid colour ID.");
                }
                ColourIDs[i] = colourID;
            }

            file.Position = currentOffset;
            ModelName = modelIDs.TryGetValue(ModelNameHash, out string carIDString) ? carIDString : $"0x{ModelNameHash:X16}";
        }


        public void WriteToCSV(CsvWriter csv)
        {
            csv.NextRecord();
            csv.WriteRecord(this);
        }

        public void WriteToGameFiles(Stream file)
        {
            file.WriteULong(ModelNameHash);
            file.WriteUInt((uint)ColourIDs.Length);
            file.WriteUInt(colourMapOffset);
        }

        public void WriteColourIDsToGameFiles(Stream file, uint colourMapStart)
        {
            colourMapOffset = (uint)file.Position - colourMapStart;
            foreach (uint colourID in ColourIDs)
            {
                file.WriteUInt(colourID);
            }
        }

        public sealed class CSVMap : ClassMap<Car>
        {
            public CSVMap()
            {
                Map(m => m.ModelName);
                Map(m => m.ColourIDs).TypeConverter(new UIntArrayConverter());
            }
        }
    }
}