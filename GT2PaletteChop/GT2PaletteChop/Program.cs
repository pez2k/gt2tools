using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

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

            if (extension == ".cdp" || extension == ".cnp" || extension == ".tex")
            {
                SplitPalettes(filename, extension == ".tex");
            }
        }

        static void SplitPalettes(string filename, bool isTEX)
        {
            TextureHeader header;
            var palettes = new List<TexturePalette>();

            using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                if (isTEX)
                {
                    header = TextureHeader.ReadFromTEXFile(infile);
                }
                else
                {
                    header = TextureHeader.ReadFromCDPFile(infile);
                }
                
                for (int i = 0; i < header.PaletteCount; i++)
                {
                    if (isTEX)
                    {
                        palettes.Add(TexturePalette.ReadFromFile<TEXPalette>(infile, i, header.PaletteIDs[i]));
                    }
                    else
                    {
                        palettes.Add(TexturePalette.ReadFromFile<CDPPalette>(infile, i, header.PaletteIDs[i]));
                    }
                }
            }

            filename = filename.Replace('.', '_');
            string paletteIDsText = "";

            foreach (TexturePalette palette in palettes)
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
            List<TexturePalette> palettes = new List<TexturePalette>();

            foreach (byte paletteID in paletteIDs.ToList())
            {
                string paletteName = basePaletteName + string.Format("{0:X2}", paletteID) + ".gtp";
                if (!File.Exists(paletteName))
                {
                    paletteIDs.Remove(paletteID);
                    continue;
                }
                
                palettes.Add(TexturePalette.ReadFromGTP(paletteName, paletteID));
            }

            using (FileStream cdpFile = new FileStream(cdpName, FileMode.Open, FileAccess.ReadWrite))
            {
                byte oldPaletteCount = (byte)cdpFile.ReadByte();
                cdpFile.Write(new byte[0x20 - 1], 0, 0x20 - 1);
                
                cdpFile.Position = 0;
                cdpFile.WriteByte((byte)palettes.Count);

                byte[] unknownData = new byte[0x40];
                if (palettes.Count > oldPaletteCount)
                {
                    cdpFile.Position = 0x220;
                    cdpFile.Read(unknownData, 0, unknownData.Length);
                }
                
                for (int i = 0; i < palettes.Count; i++)
                {
                    cdpFile.Position = 2 + i;
                    cdpFile.WriteByte(palettes[i].PaletteID);
                    
                    cdpFile.Position = 0x20 + (i * 0x240);
                    cdpFile.Write(palettes[i].PaletteData, 0, palettes[i].PaletteData.Length);

                    if (i >= oldPaletteCount)
                    {
                        cdpFile.Write(unknownData, 0, unknownData.Length);
                    }
                }

                int leftovers = oldPaletteCount - palettes.Count;
                if (leftovers > 0)
                {
                    cdpFile.Position = 0x20 + (palettes.Count * 0x240);
                    byte[] blankData = new byte[leftovers * 0x240];
                    cdpFile.Write(blankData, 0, blankData.Length);
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
        
        class TextureHeader
        {
            public byte PaletteCount { get; set; }
            public List<byte> PaletteIDs { get; set; }

            public TextureHeader()
            {
                PaletteIDs = new List<byte>();
            }

            public static TextureHeader ReadFromCDPFile(FileStream file)
            {
                return ReadFromFile(file, 0, 2);
            }

            public static TextureHeader ReadFromTEXFile(FileStream file)
            {
                return ReadFromFile(file, 0x0E, 0x10);
            }

            protected static TextureHeader ReadFromFile(FileStream file, int paletteCountPosition, int paletteIDListPosition)
            {
                var header = new TextureHeader();

                long originalPosition = file.Position;

                file.Position = paletteCountPosition;
                header.PaletteCount = (byte)file.ReadByte();
                file.Position = paletteIDListPosition;

                for (int i = 0; i < header.PaletteCount; i++)
                {
                    header.PaletteIDs.Add((byte)file.ReadByte());
                }

                file.Position = originalPosition;

                return header;
            }
        }

        class TexturePalette
        {
            public byte PaletteID { get; set; }
            public byte[] PaletteData { get; set; }
            protected static int PaletteSize { get; set; }
            protected static int PalettePosition { get; set; }

            public TexturePalette(int paletteSize, int palettePosition)
            {
                PaletteSize = paletteSize;
                PalettePosition = palettePosition;
                PaletteData = new byte[PaletteSize];
            }

            public static TPalette ReadFromFile<TPalette>(FileStream file, int paletteNumber, byte paletteID) where TPalette : TexturePalette, new()
            {
                var palette = new TPalette { PaletteID = paletteID };

                long originalPosition = file.Position;

                file.Position = PalettePosition + (paletteNumber * PaletteSize);

                file.Read(palette.PaletteData, 0, PaletteSize);

                file.Position = originalPosition;

                return palette;
            }

            public static TexturePalette ReadFromGTP(string filename, byte paletteID)
            {
                using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    TexturePalette palette;
                    if (file.Length == 0x200)
                    {
                        palette = new TEXPalette { PaletteID = paletteID };
                    }
                    else
                    {
                        palette = new CDPPalette { PaletteID = paletteID };
                    }
                    file.Position = 0;
                    file.Read(palette.PaletteData, 0, PaletteSize);
                    return palette;
                }
            }
        }

        class CDPPalette : TexturePalette
        {
            public CDPPalette() : base(0x240, 0x20)
            {
            }
        }

        class TEXPalette : TexturePalette
        {
            public TEXPalette() : base(0x200, 0x8060)
            {
            }
        }
    }
}
