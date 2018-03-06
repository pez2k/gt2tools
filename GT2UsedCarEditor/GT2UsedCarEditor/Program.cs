using System.IO;
using System.IO.Compression;

namespace GT2UsedCarEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (FileStream file = new FileStream(".usedcar", FileMode.Open, FileAccess.Read))
                {
                    using (GZipStream unzip = new GZipStream(file, CompressionMode.Decompress))
                    {
                        unzip.CopyTo(stream);
                    }
                }
                var list = new UsedCarList();
                list.Read(stream);
            }
        }
    }
}
