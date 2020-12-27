using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using StreamExtensions;

namespace GT3.CarColorEditor
{
    using HashGenerator;

    class Program
    {
        static void Main(string[] args)
        {
            var modelIDs = new Dictionary<ulong, string>() // IDs that appear in the file but aren't in any ID table
            {
                [HashGenerator.GenerateHash("ar0019")] = "ar0019",
                [HashGenerator.GenerateHash("fo0016")] = "fo0016",
                [HashGenerator.GenerateHash("fo0024")] = "fo0024",
                [HashGenerator.GenerateHash("ho0097")] = "ho0097",
                [HashGenerator.GenerateHash("ma0097")] = "ma0097",
                [HashGenerator.GenerateHash("ml0001")] = "ml0001",
                [HashGenerator.GenerateHash("ni0120")] = "ni0120",
                [HashGenerator.GenerateHash("pd0001")] = "pd0001",
                [HashGenerator.GenerateHash("ro0003")] = "ro0003",
                [HashGenerator.GenerateHash("wl1003")] = "wl1003"
            };

            var suffixes = new string[] { "", "_eu", "_us" };
            foreach (string suffix in suffixes)
            {
                string indexFile = $".id_db_idx{suffix}.db";
                string stringFile = $".id_db_str{suffix}.db";
                if (File.Exists(indexFile) && File.Exists(stringFile))
                {
                    var hashes = new IDStringTable();
                    hashes.Read(indexFile, stringFile);
                    foreach (KeyValuePair<ulong, string> hashPair in hashes.AsDictionary())
                    {
                        if (!modelIDs.ContainsKey(hashPair.Key))
                        {
                            modelIDs.Add(hashPair.Key, hashPair.Value);
                        }
                    }
                }
            }

            var colourNames = new StringTable();
            colourNames.Read("carcolor.sdb");

            using (var file = new FileStream("carcolor.db", FileMode.Open, FileAccess.Read))
            {
                // header of GT2K 00 00 00 00
                file.Position = 0x08;
                uint carCount = file.ReadUInt();
                uint carToColoursMapIndex = file.ReadUInt();
                uint coloursIndex = file.ReadUInt();
                uint fileSize = file.ReadUInt();

                using (TextWriter carOutput = new StreamWriter(File.Create("Cars.csv"), Encoding.UTF8))
                {
                    using (CsvWriter carCsv = new CsvWriter(carOutput))
                    {
                        carCsv.Configuration.QuoteAllFields = true;
                        carCsv.WriteField("ModelID");
                        carCsv.WriteField("ColourIDs");
                        carCsv.NextRecord();

                        // car entry: 8b ID hash, 4b colour count, 4b offset in map? - ordered by ID hash
                        for (uint i = 0; i < carCount; i++)
                        {
                            ulong carIDHash = file.ReadULong();
                            uint carColourCount = file.ReadUInt();
                            uint colourMapOffset = file.ReadUInt();

                            long currentOffset = file.Position;
                            file.Position = carToColoursMapIndex + colourMapOffset;

                            // map: 4b num entries, repeating 4b colour ID from colour list - ordered alphabetically by body filename?
                            var colourIDs = new uint[carColourCount];
                            for (uint j = 0; j < carColourCount; j++)
                            {
                                colourIDs[j] = file.ReadUInt();
                            }

                            file.Position = currentOffset;

                            //carCsv.WriteField(i);
                            string carID = modelIDs.TryGetValue(carIDHash, out string carIDString) ? carIDString : $"0x{carIDHash:X16}";
                            carCsv.WriteField(carID);
                            //carCsv.WriteField(carColourCount);
                            //carCsv.WriteField(colourMapOffset);
                            carCsv.WriteField(string.Join(",", colourIDs.Select(id => $"{id}")));
                            carCsv.NextRecord();
                        }
                    }
                }

                file.Position = coloursIndex;
                uint coloursCount = file.ReadUInt();

                using (TextWriter colourOutput = new StreamWriter(File.Create("Colours.csv"), Encoding.UTF8))
                {
                    using (CsvWriter colourCsv = new CsvWriter(colourOutput))
                    {
                        colourCsv.Configuration.QuoteAllFields = true;
                        colourCsv.WriteField("ColourID");
                        colourCsv.WriteField("LatinName");
                        colourCsv.WriteField("JapaneseName");
                        colourCsv.WriteField("ThumbnailColour");
                        colourCsv.NextRecord();

                        for (uint i = 0; i < coloursCount; i++)
                        {
                            uint colourNumber = file.ReadUInt();
                            uint colourStringLatin = file.ReadUInt();
                            uint colourStringJapanese = file.ReadUInt();
                            uint colourThumbnail = file.ReadUInt(); // TODO: reverse bytes

                            //colourCsv.WriteField(i);
                            colourCsv.WriteField(colourNumber);
                            colourCsv.WriteField(colourNames.Get((ushort)colourStringLatin));
                            colourCsv.WriteField(colourNames.Get((ushort)colourStringJapanese));
                            colourCsv.WriteField($"#{colourThumbnail:X6}");
                            colourCsv.NextRecord();
                        }
                    }
                }
            }
        }
    }
}
