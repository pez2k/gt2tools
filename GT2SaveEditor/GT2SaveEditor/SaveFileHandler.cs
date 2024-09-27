using System.IO;
using Force.Crc32;
using StreamExtensions;

namespace GT2.SaveEditor
{
    public static class SaveFileHandler
    {
        public static SaveFile OpenSave(string filename)
        {
            using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
            {
                int blockNumber = 1;
                int offset = blockNumber * 0x2000;
                return ReadSave(file, offset);
            }
        }

        public static SaveFile ReadSave(Stream file, int offset)
        {
            file.Position = offset;
            SaveFile save = new();
            save.ReadFromSave(file);
            return save;
        }

        public static void WriteSave(string filename, SaveFile save)
        {
            using (FileStream outfile = new(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                int blockNumber = 1;
                int offset = blockNumber * 0x2000;
                WriteSave(outfile, offset, save);
            }
        }

        public static void WriteSave(Stream file, int offset, SaveFile save)
        {
            WipeBlocks(file, offset);
            file.Position = offset;
            save.WriteToSave(file);
            ChecksumSave(file, offset);
        }

        public static void WipeBlocks(Stream file, int offset)
        {
            file.Position = offset;
            for (int i = 0; i < (0x2000 * 4); i++) // Blank the 4 blocks in the memory card for a clean write
            {
                file.WriteByte(0);
            }
        }

        public static void ChecksumSave(Stream file, int offset)
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