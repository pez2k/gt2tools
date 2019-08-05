using System.IO;

namespace GT2.TextureEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var file = new FileStream("a2azn.cnp", FileMode.Open, FileAccess.Read))
            {
                var texture = new CarTexture();
                texture.LoadFromGameFile(file, new CDPFileLayout());
            }
        }
    }
}
