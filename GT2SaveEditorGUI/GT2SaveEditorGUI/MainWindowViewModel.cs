using System;
using System.Linq;
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

        public ControllerButtonEnum[] ControllerButtonEnum { get; } = GetEnumValues<ControllerButtonEnum>();

        private static TEnum[] GetEnumValues<TEnum>() where TEnum : Enum => Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

        public void Load()
        {
            save = SaveFileHandler.OpenSave("save.mcr");
            Data = save.Data;
        }

        public void Save()
        {
            if (save != null)
            {
                SaveFileHandler.WriteSave("gt2saveeditor.mcr", save);
            }
        }
    }
}