namespace GT2.SaveEditor.Settings.Controller
{
    public enum AnalogueSteeringButtonEnum
    {
        Left = 2,
        Right,
        RightStickX = 0x80,
        RightStickY, // Not properly supported, gives broken graphics on the settings screen
        LeftStickX,
        LeftStickY // Not properly supported, gives broken graphics on the settings screen
    }
}