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

        public override void Import(string filename)
        {
            base.Import(filename);
            string file = Path.GetFileNameWithoutExtension(filename).Replace(DuplicateTag, "");
            if (!file.StartsWith("0x"))
            {
                Program.IDStrings.Add(file);
            }
        }
    }
}