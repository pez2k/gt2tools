using System.IO;
using Force.Crc32;
using StreamExtensions;

namespace GT2.SaveEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (FileStream file = new("save.mcr", FileMode.Open, FileAccess.Read))
            {
                int blockNumber = 1;
                int offset = blockNumber * 0x2000;
                SaveFile save = ReadSave(file, offset);

                using (FileStream outfile = new("gt2saveeditor.mcr", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    file.Position = 0;
                    file.CopyTo(outfile);

                    outfile.Position = offset;
                    for (int i = 0; i < (0x2000 * 4); i++) // Blank the 4 blocks in the memory card for a clean write
                    {
                        outfile.WriteByte(0);
                    }

                    WriteSave(outfile, offset, save);
                }
            }
        }

        static SaveFile ReadSave(Stream file, int offset)
        {
            file.Position = offset;
            SaveFile save = new();
            save.ReadFromSave(file);
            return save;
        }

        static void WriteSave(Stream file, int offset, SaveFile save)
        {
            file.Position = offset;
            save.WriteToSave(file);
            ChecksumSave(file, offset);
        }

        static void ChecksumSave(Stream file, int offset)
        {
            int start = offset; // Includes all header frames
            int end = offset + 0x7E9C; // Entire save data until the CRC32 checksum itself
            file.Position = start;
            byte[] buffer = new byte[end - start];
            file.Read(buffer);
            uint checksum = Crc32Algorithm.Compute(buffer);
            file.WriteUInt(checksum);
        }
    }
}