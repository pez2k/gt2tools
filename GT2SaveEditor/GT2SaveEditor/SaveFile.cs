using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor
{
    using Garage;
    using License;
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
        public uint Day { get; set; }
        public LicenseData SLicense { get; set; } = new();
        public LicenseData IALicense { get; set; } = new();
        public LicenseData IBLicense { get; set; } = new();
        public LicenseData ICLicense { get; set; } = new();
        public LicenseData ALicense { get; set; } = new();
        public LicenseData BLicense { get; set; } = new();
        public GarageData Garage { get; set; } = new();

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
            Day = file.ReadUInt();

            file.Position += 0x131C;
            SLicense.ReadFromSave(file);
            IALicense.ReadFromSave(file);
            IBLicense.ReadFromSave(file);
            ICLicense.ReadFromSave(file);
            ALicense.ReadFromSave(file);
            BLicense.ReadFromSave(file);

            file.Position += 0x1EC;
            Garage.ReadFromSave(file);
        }
    }
}