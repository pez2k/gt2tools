using System.IO;

namespace GT2.TextureEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            Dump(args[0]);
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
    }
}
