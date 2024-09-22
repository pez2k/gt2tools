using System.IO;

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
            HeaderFrame.WriteToSave(file);
            IconFrame1.WriteToSave(file);
            IconFrame2.WriteToSave(file);
            IconFrame3.WriteToSave(file);
            Data.WriteToSave(file);
        }
    }
}