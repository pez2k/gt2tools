using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor
{
    public class ArcadeSettings
    {
        public bool DamageEnabled { get; set; }
        public byte Laps { get; set; }

        public void ReadFromSave(Stream file)
        {
            DamageEnabled = file.ReadSingleByte() == 1;
            Laps = file.ReadSingleByte();
        }
    }
}