using System.IO;

namespace GT2.SaveEditor.Settings.Controller
{
    public class ControllerSettings
    {
        public DigitalControllerSettings DigitalController { get; set; } = new();
        public AnalogueControllerSettings AnalogueController { get; set; } = new();
        public NegconControllerSettings NegconController { get; set; } = new();
        public UnknownControllerSettings UnknownController { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            DigitalController.ReadBindingsFromSave(file);
            AnalogueController.ReadBindingsFromSave(file);
            NegconController.ReadBindingsFromSave(file);
            UnknownController.ReadBindingsFromSave(file);
            AnalogueController.ReadSettingsFromSave(file);
            NegconController.ReadSettingsFromSave(file);
            UnknownController.ReadSettingsFromSave(file);
        }

        public void WriteToSave(Stream file)
        {
            DigitalController.WriteBindingsToSave(file);
            AnalogueController.WriteBindingsToSave(file);
            NegconController.WriteBindingsToSave(file);
            UnknownController.WriteBindingsToSave(file);
            AnalogueController.WriteSettingsToSave(file);
            NegconController.WriteSettingsToSave(file);
            UnknownController.WriteSettingsToSave(file);
        }
    }
}