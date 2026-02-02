using StreamExtensions;

namespace GT2BillboardEditor
{
    internal class Billboard
    {
        public ushort Brand { get; set; }
        public ushort Chance { get; set; }

        public void ReadFromFile(Stream file)
        {
            Chance = file.ReadUShort();
            Brand = file.ReadUShort();
        }

        public void WriteToFile(Stream file)
        {
            file.WriteUShort(Chance);
            file.WriteUShort(Brand);
        }
    }
}
