using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.License
{
    public class LicenseTestRecord
    {
        public int Time { get; set; }
        public ushort Speed { get; set; }
        public string Name { get; set; } = "";

        public void ReadTimeAndSpeedFromSave(Stream file)
        {
            Time = file.ReadInt();
            file.Position += 0xC;
            Speed = file.ReadUShort();
            file.Position += 0x2;
        }

        public void ReadNameFromSave(Stream file)
        {
            long stringStart = file.Position;
            Name = file.ReadCharacters();
            file.Position = stringStart + 12;
        }
    }
}