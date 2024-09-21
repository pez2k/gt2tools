using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Settings
{
    public class ArcadeSettings
    {
        public bool DamageEnabled { get; set; }
        public byte Laps { get; set; }

        public void ReadFromSave(Stream file)
        {
            DamageEnabled = file.ReadByteAsBool();
            Laps = file.ReadSingleByte();
        }

        public void WriteToSave(Stream file)
        {
            file.WriteBoolAsByte(DamageEnabled);
            file.WriteByte(Laps);
        }
    }
}