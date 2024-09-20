using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor
{
    public class SaveFile
    {
        public LanguageEnum Language { get; set; }
        public ArcadeSettings ArcadeSettings { get; set; } = new();
        public TwoPlayerSettings TwoPlayerSettings { get; set; } = new();
        public ControllerSettings Player1Controller { get; set; } = new();
        public ControllerSettings Player2Controller { get; set; } = new();
        public GlobalSettings GlobalSettings { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            Language = (LanguageEnum)file.ReadSingleByte();

            file.Position += 1;
            ArcadeSettings.ReadFromSave(file);
            TwoPlayerSettings.ReadFromSave(file);

            file.Position += 1;
            Player1Controller.ReadFromSave(file);

            file.Position += 4;
            Player2Controller.ReadFromSave(file);

            file.Position += 4;
            GlobalSettings.ReadFromSave(file);
        }
    }
}