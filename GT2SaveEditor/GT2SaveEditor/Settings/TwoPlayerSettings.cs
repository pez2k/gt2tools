using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Settings
{
    public class TwoPlayerSettings
    {
        public TireDamageEnum TireDamage { get; set; }
        public bool DamageEnabled { get; set; }
        public byte Laps { get; set; }
        public sbyte Handicap { get; set; }
        public BoostEnum Boost { get; set; }

        public void ReadFromSave(Stream file)
        {
            TireDamage = (TireDamageEnum)file.ReadSingleByte();
            DamageEnabled = file.ReadByteAsBool();
            Laps = file.ReadSingleByte();
            Handicap = file.ReadSByte();
            Boost = (BoostEnum)file.ReadSingleByte();
            file.Position += 0x1;
        }

        public void WriteToSave(Stream file)
        {
            file.WriteByte((byte)TireDamage);
            file.WriteBoolAsByte(DamageEnabled);
            file.WriteByte(Laps);
            file.WriteSByte(Handicap);
            file.WriteByte((byte)Boost);
            file.Position += 0x1;
        }
    }
}