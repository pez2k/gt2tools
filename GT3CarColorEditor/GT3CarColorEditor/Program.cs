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
    class Program
    {
        static void Main(string[] args)
        {
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

                            carCsv.WriteField(i);
                            carCsv.WriteField($"0x{carIDHash:X16}");
                            carCsv.WriteField(carColourCount);
                            //carCsv.WriteField(colourMapOffset);
                            carCsv.WriteField(colourIDs);
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
                        for (uint i = 0; i < coloursCount; i++)
                        {
                            uint colourNumber = file.ReadUInt();
                            uint colourStringLatin = file.ReadUInt();
                            uint colourStringJapanese = file.ReadUInt();
                            uint colourThumbnail = file.ReadUInt();

                            colourCsv.WriteField(i);
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
