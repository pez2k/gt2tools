using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode.License
{
    public class LicenseTestRecord
    {
        public int TotalTime { get; set; }
        public int Sector1Time { get; set; }
        public int Sector2Time { get; set; }
        public int Sector3Time { get; set; }
        public ushort Speed { get; set; }
        public string Name { get; set; } = "";

        public void ReadTimeAndSpeedFromSave(Stream file)
        {
            TotalTime = file.ReadInt();
            Sector1Time = file.ReadInt();
            Sector2Time = file.ReadInt();
            Sector3Time = file.ReadInt();
            Speed = file.ReadUShort();
            file.Position += 0x2;
        }

        public void ReadNameFromSave(Stream file)
        {
            long stringStart = file.Position;
            Name = file.ReadCharacters();
            file.Position = stringStart + 12;
        }

        public void WriteTimeAndSpeedToSave(Stream file)
        {
            file.WriteInt(TotalTime);
            file.WriteInt(Sector1Time);
            file.WriteInt(Sector2Time);
            file.WriteInt(Sector3Time);
            file.WriteUShort(Speed);
            file.WriteUShort(0xFFFF);
        }

        public void WriteNameToSave(Stream file)
        {
            long stringStart = file.Position;
            file.WriteCharacters(Name);
            while (file.Position < stringStart + 12)
            {
                file.WriteByte(0);
            }
        }
    }
}