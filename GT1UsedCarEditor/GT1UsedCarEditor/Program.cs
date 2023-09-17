using StreamExtensions;

namespace GT1.UsedCarEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            FileAttributes attributes = File.GetAttributes(args[0]);

            if (attributes.HasFlag(FileAttributes.Directory))
            {
                if (Directory.Exists(args[0]))
                {
                    //WriteFile(args[0]);
                }
            }
            else if (File.Exists(args[0]))
            {
                ReadFile(args[0]);
            }
        }


        static void ReadFile(string path)
        {
            UsedCarList list;
            using (FileStream file = new(path, FileMode.Open, FileAccess.Read))
            {
                string magic = file.ReadCharacters();
                if (magic != "@(#)USEDCAR")
                {
                    return;
                }
                list = UsedCarList.ReadFromFile(file);
            }

            string directory = Path.GetFileNameWithoutExtension(path);
            Directory.CreateDirectory(directory);
            list.WriteToCSV(directory);
        }
    }
}