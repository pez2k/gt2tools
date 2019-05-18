using System.IO;

namespace GT3.DataSplitter
{
    using StreamExtensions;

    public class CarDataStructure : DataStructure
    {
        public override string CreateOutputFilename(byte[] data)
        {
            ulong hexID = data.ReadULong();
            string id = Program.IDStrings.Get(hexID);
            return Path.Combine(Name, $"{id}.dat");
        }
    }
}