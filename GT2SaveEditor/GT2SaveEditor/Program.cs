using System.IO;

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
                SaveFile save = SaveFileHandler.ReadSave(file, offset);

                using (FileStream outfile = new("gt2saveeditor.mcr", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    file.Position = 0;
                    file.CopyTo(outfile);

                    SaveFileHandler.WriteSave(outfile, offset, save);
                }
            }
        }
    }
}