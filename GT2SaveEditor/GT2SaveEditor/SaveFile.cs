using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor
{
    public class SaveFile
    {
        public SaveFrame HeaderFrame { get; set; } = new();
        public SaveFrame IconFrame1 { get; set; } = new();
        public SaveFrame IconFrame2 { get; set; } = new();
        public SaveFrame IconFrame3 { get; set; } = new();
        public SaveData Data { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            HeaderFrame.ReadFromSave(file);
            IconFrame1.ReadFromSave(file);
            IconFrame2.ReadFromSave(file);
            IconFrame3.ReadFromSave(file);
            Data.ReadFromSave(file);
        }

        public void WriteToSave(Stream file)
        {
            long startPosition = file.Position;

            HeaderFrame.WriteToSave(file);
            IconFrame1.WriteToSave(file);
            IconFrame2.WriteToSave(file);
            IconFrame3.WriteToSave(file);
            Data.WriteToSave(file);

            file.MoveToNextMultipleOf(0x80); // Find the end of the frame now that we've finished writing data
            for (int i = 0; i < (startPosition + (0x2000 * 4)); i++) // Blank the remaining frames up to the save size of 4 blocks - this should always be 2 frames (0x100)
            {
                file.WriteByte(0xFF);
            }
        }
    }
}