using System.IO;

namespace GT2.SaveEditor.Settings.Controller
{
    public class ControllerSettings
    {
        public DigitalControllerSettings DigitalController { get; set; } = new();
        public AnalogueControllerSettings AnalogueController { get; set; } = new();
        public NegconControllerSettings NegconController { get; set; } = new();

        public void ReadFromSave(Stream file)
        {
            DigitalController.ReadFromSave(file);
            file.Position += 0x1;
            AnalogueController.ReadFromSave(file);
            file.Position += 0x9;
            NegconController.ReadFromSave(file);
        }
    }
}