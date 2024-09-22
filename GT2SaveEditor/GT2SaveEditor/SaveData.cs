using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor
{
    using Arcade;
    using GTMode;
    using Settings;
    using Settings.Controller;

    public class SaveData
    {
        public LanguageEnum Language { get; set; }
        public ArcadeSettings ArcadeSettings { get; set; } = new();
        public TwoPlayerSettings TwoPlayerSettings { get; set; } = new();
        public ControllerSettings Player1Controller { get; set; } = new();
        public ControllerSettings Player2Controller { get; set; } = new();
        public GlobalSettings GlobalSettings { get; set; } = new();
        public ArcadeResults ArcadeResults { get; set; } = new();
        public GTModeStats GTModeStats { get; set; } = new();
        public ArcadeRecords ArcadeRecords { get; set; } = new();
        public GTModeProgress GTModeProgress { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            Language = (LanguageEnum)file.ReadSingleByte();
            file.Position += 0x1;

            ArcadeSettings.ReadFromSave(file);
            TwoPlayerSettings.ReadFromSave(file);
            Player1Controller.ReadFromSave(file);
            Player2Controller.ReadFromSave(file);
            GlobalSettings.ReadFromSave(file);
            ArcadeResults.ReadFromSave(file);

            file.Position += 0x28;
            GTModeStats.ReadFromSave(file);
            ArcadeRecords.ReadFromSave(file);

            file.Position += 0x48;
            GTModeProgress.ReadFromSave(file);
        }

        public void WriteToSave(Stream file)
        {
            file.WriteByte((byte)Language);
            file.Position += 0x1;

            ArcadeSettings.WriteToSave(file);
            TwoPlayerSettings.WriteToSave(file);
            Player1Controller.WriteToSave(file);
            Player2Controller.WriteToSave(file);
            GlobalSettings.WriteToSave(file);
            ArcadeResults.WriteToSave(file);

            file.Position += 0x28;
            GTModeStats.WriteToSave(file);
            ArcadeRecords.WriteToSave(file);

            file.Position += 0x48;
            GTModeProgress.WriteToSave(file);
        }
    }
}