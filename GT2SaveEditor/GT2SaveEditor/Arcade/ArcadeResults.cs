using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Arcade
{
    public class ArcadeResults
    {
        public ArcadeCompletionEnum[] Results { get; set; } = new ArcadeCompletionEnum[21];

        public void ReadFromSave(Stream file)
        {
            for (int i = 0; i < Results.Length; i++)
            {
                Results[i] = (ArcadeCompletionEnum)file.ReadSingleByte();
            }
            file.Position += 0x3;
        }

        public void WriteToSave(Stream file)
        {
            for (int i = 0; i < Results.Length; i++)
            {
                file.WriteByte((byte)Results[i]);
            }
            file.Position += 0x3;
        }

        // Order of results:
        // Rome
        // Rome Short
        // Rome-Night
        // Seattle
        // Seattle Short
        // Super Speedway
        // Laguna Seca
        // Midfield
        // Apricot Hill
        // Red Rock Valley
        // Tahiti Road
        // High Speed Ring
        // Autumn Ring
        // Trial Mountain
        // Deep Forest
        // Grand Valley
        // Grand Valley East
        // SSR5
        // CSR5
        // Grindelwald
        // Test Course
    }
}