﻿using System.IO;

namespace GT2.TextureEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            string path = args[0];
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                Build(path);
            }
            else
            {
                Dump(path);
            }
        }

        static void Dump(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var texture = new CarTexture();
                texture.LoadFromGameFile(file, new CDPFileLayout());
                string outputName = Path.GetFileNameWithoutExtension(filename);
                string outputPath = Path.Combine(Path.GetDirectoryName(filename), outputName);
                Directory.CreateDirectory(outputPath);
                using (var bitmap = new FileStream(Path.Combine(outputPath, $"{outputName}.bmp"), FileMode.Create, FileAccess.Write))
                {
                    texture.WriteToEditableFiles(outputPath, bitmap);
                }
            }
        }

        static void Build(string directory)
        {
            string carName = Path.GetFileName(directory);

            using (var file = new FileStream($"{carName}.cdp", FileMode.Create, FileAccess.Write))
            {
                var texture = new CarTexture();
                using (var bitmap = new FileStream(Path.Combine(carName, $"{carName}.bmp"), FileMode.Open, FileAccess.Read))
                {
                    texture.LoadFromEditableFiles(directory, bitmap);
                }
                texture.WriteToGameFile(file, new CDPFileLayout());
            }
        }
    }
}
