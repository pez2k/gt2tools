using StreamExtensions;

namespace GT1.UsedCarEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage:\r\nGT1UsedCarEditor <used car file>\r\nOR\r\nGT1UsedCarEditor <CSV directory>");
                return;
            }

            FileAttributes attributes = File.GetAttributes(args[0]);

            if (attributes.HasFlag(FileAttributes.Directory))
            {
                if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine("Directory does not exist");
                    return;
                }
                WriteFile(args[0]);
            }
            else if (File.Exists(args[0]))
            {
                ReadFile(args[0]);
            }
            else
            {
                Console.WriteLine("File does not exist");
            }
        }

        private static void ReadFile(string path)
        {
            UsedCarList list;
            using (FileStream file = new(path, FileMode.Open, FileAccess.Read))
            {
                string magic = file.ReadCharacters();
                if (magic != "@(#)USEDCAR")
                {
                    Console.WriteLine("Not a USEDCAR file");
                    return;
                }
                file.Position = 0xE;
                if (file.ReadUShort() != 6)
                {
                    Console.WriteLine("Unexpected header value");
                    return;
                }
                list = UsedCarList.ReadFromFile(file);
            }

            string directory = Path.GetFileNameWithoutExtension(path);
            Directory.CreateDirectory(directory);
            list.WriteToCSV(directory);
        }

        private static void WriteFile(string path)
        {
            UsedCarList list = UsedCarList.ReadFromCSV(path);
            using (FileStream file = new($"{Path.GetFileNameWithoutExtension(path)}.usedcar", FileMode.Create, FileAccess.Write))
            {
                file.WriteCharacters("@(#)USEDCAR");
                file.Position = 0xE;
                file.WriteUShort(6); // manufacturer count?
                list.WriteToFile(file);
            }
        }
    }
}