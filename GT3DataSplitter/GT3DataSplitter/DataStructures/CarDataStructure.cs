using System.IO;

namespace GT3.DataSplitter
{
    using StreamExtensions;

    public class CarDataStructure : DataStructure
    {
        public override string CreateOutputFilename(byte[] data)
        {
            ulong hexID = data.ReadULong();
            string filename = Program.IDStrings.Get(hexID) ?? GetFileCount();
            return Path.Combine(Name, $"{filename}.dat");
        }

        private string GetFileCount() => $"{Directory.GetFiles(Name).Length:D4}";
    }
}