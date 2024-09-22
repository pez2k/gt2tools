using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode
{
    using Garage;
    using License;

    public class GTModeData
    {
        public uint Day { get; set; }
        public uint TotalRaces { get; set; }
        public uint TotalWins { get; set; }
        public uint SumOfBestPossibleRankings { get; set; } // Exactly the same as total races, because it's total races * 1 for 1st place - divided by the below to get avg ranking stat
        public uint SumOfRankings { get; set; } // e.g. 1 + 3 + 5 for 3 races finishing 1st, 3rd, and 5th - guaranteed to be equal to or higher than the above
        public uint TotalWinnings { get; set; }
        public EventResults EventResults { get; set; } = new();
        public bool EndingMovieUnlocked { get; set; }
        public LicenseData SLicense { get; set; } = new();
        public LicenseData IALicense { get; set; } = new();
        public LicenseData IBLicense { get; set; } = new();
        public LicenseData ICLicense { get; set; } = new();
        public LicenseData ALicense { get; set; } = new();
        public LicenseData BLicense { get; set; } = new();
        public GarageData Garage { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            Day = file.ReadUInt();
            file.Position += 0x4;
            TotalRaces = file.ReadUInt();
            TotalWins = file.ReadUInt();
            SumOfBestPossibleRankings = file.ReadUInt();
            SumOfRankings = file.ReadUInt();
            file.Position += 0x4;
            TotalWinnings = file.ReadUInt();
            EventResults.ReadFromSave(file);

            file.Position += 0x7D;
            EndingMovieUnlocked = file.ReadByteAsBool();
            file.Position += 0x2;

            file.Position += 0x1200; // Event records most likely
            SLicense.ReadFromSave(file);
            IALicense.ReadFromSave(file);
            IBLicense.ReadFromSave(file);
            ICLicense.ReadFromSave(file);
            ALicense.ReadFromSave(file);
            BLicense.ReadFromSave(file);

            file.Position += 0x1EC;
            Garage.ReadFromSave(file);
        }

        public void WriteToSave(Stream file)
        {
            file.WriteUInt(Day);
            file.Position += 0x4;
            file.WriteUInt(TotalRaces);
            file.WriteUInt(TotalWins);
            file.WriteUInt(SumOfBestPossibleRankings);
            file.WriteUInt(SumOfRankings);
            file.Position += 0x4;
            file.WriteUInt(TotalWinnings);
            EventResults.WriteToSave(file);

            file.Position += 0x7D;
            file.WriteBoolAsByte(EndingMovieUnlocked);
            file.Position += 0x2;

            file.Position += 0x1200;
            SLicense.WriteToSave(file);
            IALicense.WriteToSave(file);
            IBLicense.WriteToSave(file);
            ICLicense.WriteToSave(file);
            ALicense.WriteToSave(file);
            BLicense.WriteToSave(file);

            file.Position += 0x1EC;
            Garage.WriteToSave(file);
        }
    }
}