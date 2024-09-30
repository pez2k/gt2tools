using System;
using System.Collections.ObjectModel;
using System.Linq;
using GT2.SaveEditor.GTMode.License;
using GT2.SaveEditor.Settings;
using GT2.SaveEditor.Settings.Controller;

namespace GT2.SaveEditor.GUI
{
    public class MainWindowViewModel
    {
        private SaveFile? save;

        public SaveData? Data { get; set; }

        public LanguageEnum[] LanguageEnum { get; } = GetEnumValues<LanguageEnum>();
        public ReplayInfoEnum[] ReplayInfoEnum { get; } = GetEnumValues<ReplayInfoEnum>();
        public CameraPositionEnum[] CameraPositionEnum { get; } = GetEnumValues<CameraPositionEnum>();
        public ChaseViewEnum[] ChaseViewEnum { get; } = GetEnumValues<ChaseViewEnum>();
        public ViewAngleEnum[] ViewAngleEnum { get; } = GetEnumValues<ViewAngleEnum>();
        public TireDamageEnum[] TireDamageEnum { get; } = GetEnumValues<TireDamageEnum>();
        public BoostEnum[] BoostEnum { get; } = GetEnumValues<BoostEnum>();

        public ControllerButtonEnum[] ControllerButtonEnum { get; } = GetEnumValues<ControllerButtonEnum>();
        public AnalogueSteeringButtonEnum[] AnalogueSteeringButtonEnum { get; } = GetEnumValues<AnalogueSteeringButtonEnum>();
        public AnalogueAccelerateBrakeButtonEnum[] AnalogueAccelerateBrakeButtonEnum { get; } = GetEnumValues<AnalogueAccelerateBrakeButtonEnum>();
        public NegconSteeringButtonEnum[] NegconSteeringButtonEnum { get; } = GetEnumValues<NegconSteeringButtonEnum>();
        public NegconAccelerateBrakeButtonEnum[] NegconAccelerateBrakeButtonEnum { get; } = GetEnumValues<NegconAccelerateBrakeButtonEnum>();
        public NegconButtonEnum[] NegconButtonEnum { get; } = GetEnumValues<NegconButtonEnum>();

        public ObservableCollection<LicenseTestViewModel>? SLicense { get; set; }
        public ObservableCollection<LicenseTestViewModel>? IALicense { get; set; }
        public ObservableCollection<LicenseTestViewModel>? IBLicense { get; set; }
        public ObservableCollection<LicenseTestViewModel>? ICLicense { get; set; }
        public ObservableCollection<LicenseTestViewModel>? ALicense { get; set; }
        public ObservableCollection<LicenseTestViewModel>? BLicense { get; set; }

        private static TEnum[] GetEnumValues<TEnum>() where TEnum : Enum => Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

        public void Load()
        {
            save = SaveFileHandler.OpenSave("save.mcr");
            Data = save.Data;
            SLicense  = GenerateLicenseViewModels("S",  Data.GTModeProgress.SLicense.Tests);
            IALicense = GenerateLicenseViewModels("IA", Data.GTModeProgress.IALicense.Tests);
            IBLicense = GenerateLicenseViewModels("IB", Data.GTModeProgress.IBLicense.Tests);
            ICLicense = GenerateLicenseViewModels("IC", Data.GTModeProgress.ICLicense.Tests);
            ALicense  = GenerateLicenseViewModels("A",  Data.GTModeProgress.ALicense.Tests);
            BLicense  = GenerateLicenseViewModels("B",  Data.GTModeProgress.BLicense.Tests);
        }

        private static ObservableCollection<LicenseTestViewModel> GenerateLicenseViewModels(string license, LicenseTestData[] tests) =>
            new ObservableCollection<LicenseTestViewModel>(Enumerable.Range(0, tests.Length).Select(i => new LicenseTestViewModel($"{license}-{i + 1}", tests[i])));

        public void Save()
        {
            if (save != null)
            {
                SaveFileHandler.WriteSave("gt2saveeditor.mcr", save);
            }
        }
    }
}