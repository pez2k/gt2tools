using System;
using System.IO;
using StreamExtensions;

namespace GT2.TextureEditor
{
    class IlluminationMask
    {
        private readonly bool[] colours = new bool[16];

        public bool IsEmpty {
            get
            {
                foreach (bool colour in colours)
                {
                    if (colour)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void LoadFromGameFile(Stream file)
        {
            ushort flags = file.ReadUShort();
            for (int i = 0; i < 16; i++)
            {
                colours[i] = (flags & 1) == 1;
                flags = (ushort)(flags >> 1);
            }
        }

        public void WriteToJASCPalette(Stream file)
        {
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine("JASC-PAL");
                writer.WriteLine("0100");
                writer.WriteLine("16");
                foreach (bool colour in colours)
                {
                    if (colour)
                    {
                        writer.WriteLine($"248 248 248");
                    }
                    else
                    {
                        writer.WriteLine($"0 0 0");
                    }
                }
            }
        }

        public void LoadFromEditableFile(Stream file)
        {
            using (var reader = new StreamReader(file))
            {
                if (reader.ReadLine() != "JASC-PAL" || reader.ReadLine() != "0100" || reader.ReadLine() != "16")
                {
                    throw new Exception("Invalid JASC palette.");
                }

                for (int i = 0; i < 16; i++)
                {
                    string colourText = reader.ReadLine();
                    colours[i] = colourText != "0 0 0";
                }
            }
        }
    }
}
