using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor
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
            TyreWearEnabled = file.ReadSingleByte() == 1;
            DamageEnabled = file.ReadSingleByte() == 1;
            Laps = file.ReadSingleByte();
            Handicap = file.ReadSingleByte();
            Boost = file.ReadSingleByte();
        }
    }
}