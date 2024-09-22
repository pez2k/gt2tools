using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.GTMode
{
    public class GTModeStats
    {
        public uint Day { get; set; }
        public uint TotalRaces { get; set; }
        public uint TotalWins { get; set; }
        public uint SumOfBestPossibleRankings { get; set; } // Exactly the same as total races, because it's total races * 1 for 1st place - divided by the below to get avg ranking stat
        public uint SumOfRankings { get; set; } // e.g. 1 + 3 + 5 for 3 races finishing 1st, 3rd, and 5th - guaranteed to be equal to or higher than the above
        public uint TotalWinnings { get; set; }
        public EventResults EventResults { get; set; } = new();
        public bool EndingMovieUnlocked { get; set; }

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
        }
    }
}