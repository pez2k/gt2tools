using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2.PaletteChop
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            string filename = args[0];
            string extension = Path.GetExtension(filename);

            if (extension == ".txt")
            {
                BuildCDP(filename);
                return;
            }

            if (extension == ".gz")
            {
                string innerFilename = Path.GetFileNameWithoutExtension(filename);
                extension = Path.GetExtension(innerFilename);

                if (extension != ".cdp" && extension != ".cnp")
                {
                    return;
                }

                using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    // Un-gzip
                    using (GZipStream unzip = new GZipStream(infile, CompressionMode.Decompress))
                    {
                        filename = innerFilename;
                        using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                        {
                            unzip.CopyTo(outfile);
                        }
                    }
                }
            }

            if (extension == ".cdp" || extension == ".cnp")
            {
                SplitPalettes(filename);
            }
        }

        static void SplitPalettes(string filename)
        {
            CDPHeader header;
            var palettes = new List<CDPPalette>();

            using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                header = CDPHeader.ReadFromFile(infile);
                
                for (int i = 0; i < header.PaletteCount; i++)
                {
                    palettes.Add(CDPPalette.ReadFromFile(infile, i, header.PaletteIDs[i]));
                }
            }

            filename = filename.Replace('.', '_');
            string paletteIDsText = "";

            foreach (CDPPalette palette in palettes)
            {
                string paletteID = string.Format("{0:X2}", palette.PaletteID);
                paletteIDsText += paletteID + "\r\n";

                using (FileStream paletteFile = new FileStream(filename + "_" + paletteID + ".gtp", FileMode.Create, FileAccess.Write))
                {
                    paletteFile.Write(palette.PaletteData, 0, palette.PaletteData.Length);
                }
            }

            using (FileStream indexFile = new FileStream(filename + ".txt", FileMode.Create, FileAccess.Write))
            {
                byte[] stringBytes = Encoding.ASCII.GetBytes(paletteIDsText.ToCharArray());
                indexFile.Write(stringBytes, 0, stringBytes.Length);
            }
        }

        static void BuildCDP(string filename)
        {
            string cdpName = Path.GetFileNameWithoutExtension(filename).Replace("_cdp", ".cdp").Replace("_cnp", ".cnp");

            if (!File.Exists(cdpName))
            {
                return;
            }

            List<byte> paletteIDs = new List<byte>();

            using (StreamReader indexFile = File.OpenText(filename))
            {
                while (!indexFile.EndOfStream)
                {
                    byte paletteID;
                    if (!byte.TryParse(indexFile.ReadLine(), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out paletteID))
                    {
                        continue;
                    }
                    paletteIDs.Add(paletteID);
                }
            }

            string basePaletteName = Path.GetFileNameWithoutExtension(filename) + "_";
            List<CDPPalette> palettes = new List<CDPPalette>();

            foreach (byte paletteID in paletteIDs.ToList())
            {
                string paletteName = basePaletteName + string.Format("{0:X2}", paletteID) + ".gtp";
                if (!File.Exists(paletteName))
                {
                    paletteIDs.Remove(paletteID);
                    continue;
                }
                
                palettes.Add(CDPPalette.ReadFromGTP(paletteName, paletteID));
            }

            using (FileStream cdpFile = new FileStream(cdpName, FileMode.Open, FileAccess.ReadWrite))
            {
                byte oldPaletteCount = (byte)cdpFile.ReadByte();
                cdpFile.Write(new byte[0x20 - 1], 0, 0x20 - 1);

                {
                    byte[] blankData = new byte[oldPaletteCount * 0x240];
                    cdpFile.Write(blankData, 0, blankData.Length);
                }

                cdpFile.Position = 0;
                cdpFile.WriteByte((byte)palettes.Count);

                for (int i = 0; i < palettes.Count; i++)
                {
                    cdpFile.Position = 2 + i;
                    cdpFile.WriteByte(palettes[i].PaletteID);

                    cdpFile.Position = 0x20 + (i * 0x240);
                    cdpFile.Write(palettes[i].PaletteData, 0, palettes[i].PaletteData.Length);
                }

                cdpFile.Position = 0;
                
                using (FileStream zipFile = new FileStream(cdpName + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new GZipStream(zipFile, CompressionMode.Compress))
                    {
                        cdpFile.CopyTo(zip);
                    }
                }
            }
        }

        class CDPHeader
        {
            public byte PaletteCount { get; set; }
            public List<byte> PaletteIDs { get; set; }

            public CDPHeader()
            {
                PaletteIDs = new List<byte>();
            }

            public static CDPHeader ReadFromFile(FileStream file)
            {
                var header = new CDPHeader();

                long originalPosition = file.Position;

                file.Position = 0;
                header.PaletteCount = (byte)file.ReadByte();
                file.Position = 2;
                
                for (int i = 0; i < header.PaletteCount; i++)
                {
                    header.PaletteIDs.Add((byte)file.ReadByte());
                }

                file.Position = originalPosition;

                return header;
            }
        }

        class CDPPalette
        {
            public byte PaletteID { get; set; }
            public byte[] PaletteData { get; set; }

            public CDPPalette()
            {
                PaletteData = new byte[0x240];
            }

            public static CDPPalette ReadFromFile(FileStream file, int paletteNumber, byte paletteID)
            {
                var palette = new CDPPalette { PaletteID = paletteID };

                long originalPosition = file.Position;

                file.Position = 0x20 + (paletteNumber * 0x240);

                file.Read(palette.PaletteData, 0, 0x240);

                file.Position = originalPosition;

                return palette;
            }

            public static CDPPalette ReadFromGTP(string filename, byte paletteID)
            {
                using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    var palette = new CDPPalette { PaletteID = paletteID };
                    file.Position = 0;
                    file.Read(palette.PaletteData, 0, 0x240);
                    return palette;
                }
            }
        }
    }
}
