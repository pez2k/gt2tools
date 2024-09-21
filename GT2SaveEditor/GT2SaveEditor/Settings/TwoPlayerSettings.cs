using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Settings
{
    public class TwoPlayerSettings
    {
        public bool TyreWearEnabled { get; set; }
        public bool DamageEnabled { get; set; }
        public byte Laps { get; set; }
        public byte Handicap { get; set; }
        public byte Boost { get; set; }

        public void ReadFromSave(Stream file)
        {
            TyreWearEnabled = file.ReadByteAsBool();
            DamageEnabled = file.ReadByteAsBool();
            Laps = file.ReadSingleByte();
            Handicap = file.ReadSingleByte();
            Boost = file.ReadSingleByte();
        }

        public void WriteToSave(Stream file)
        {
            file.WriteBoolAsByte(TyreWearEnabled);
            file.WriteBoolAsByte(DamageEnabled);
            file.WriteByte(Laps);
            file.WriteByte(Handicap);
            file.WriteByte(Boost);
        }
    }
}