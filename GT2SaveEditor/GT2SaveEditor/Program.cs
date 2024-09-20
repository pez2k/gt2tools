using System.IO;

namespace GT2.SaveEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (FileStream file = new("save.mcr", FileMode.Open, FileAccess.Read))
            {
                ReadSave(file, 0x2000);
            }
        }

        static void ReadSave(Stream file, int offset)
        {
            file.Position = offset + 0x200;
            SaveFile save = new();
            save.ReadFromSave(file);
        }
    }
}