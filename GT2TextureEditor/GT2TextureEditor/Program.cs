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
                using (var bitmap = new FileStream($"{Path.GetFileNameWithoutExtension(filename)}.bmp", FileMode.Create, FileAccess.Write))
                {
                    texture.WriteToEditableFiles(bitmap);
                }
            }
        }
    }
}
