using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor
{
    using GTMode;
    using Settings;
    using Settings.Controller;

    public class SaveFile
    {
        public LanguageEnum Language { get; set; }
        public ArcadeSettings ArcadeSettings { get; set; } = new();
        public TwoPlayerSettings TwoPlayerSettings { get; set; } = new();
        public ControllerSettings Player1Controller { get; set; } = new();
        public ControllerSettings Player2Controller { get; set; } = new();
        public GlobalSettings GlobalSettings { get; set; } = new();
        public GTModeData GTModeData { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            Language = (LanguageEnum)file.ReadSingleByte();

            file.Position += 0x1;
            ArcadeSettings.ReadFromSave(file);
            TwoPlayerSettings.ReadFromSave(file);

            file.Position += 0x1;
            Player1Controller.ReadFromSave(file);

            file.Position += 0x4;
            Player2Controller.ReadFromSave(file);

            file.Position += 0x4;
            GlobalSettings.ReadFromSave(file);

            file.Position += 0x43;
            GTModeData.ReadFromSave(file);
        }

        public void WriteToSave(Stream file)
        {
            file.WriteByte((byte)Language);

            file.Position += 0x1;
            ArcadeSettings.WriteToSave(file);
            TwoPlayerSettings.WriteToSave(file);

            file.Position += 0x1;
            Player1Controller.WriteToSave(file);

            file.Position += 0x4;
            Player2Controller.WriteToSave(file);

            file.Position += 0x4;
            GlobalSettings.WriteToSave(file);

            file.Position += 0x43;
            GTModeData.WriteToSave(file);
        }
    }
}