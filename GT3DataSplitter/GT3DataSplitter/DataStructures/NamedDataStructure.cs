using System.IO;

namespace GT3.DataSplitter
{
    using StreamExtensions;

    public class NamedDataStructure : DataStructure
    {
        public override string CreateOutputFilename(byte[] data)
        {
            ulong hexID = data.ReadULong();
            string filename = Program.IDStrings.Get(hexID) ?? $"0x{hexID:X16}";
            return Path.Combine(Name, $"{filename}.dat");
        }
    }
}