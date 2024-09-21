namespace GT2.SaveEditor.Settings.Controller
{
    public enum AnalogueAccelerateBrakeButtonEnum
    {
        Up,
        Down,
        Left,
        Right,
        L1,
        L2,
        L3, // Not properly supported, gives broken graphics on the settings screen
        Triangle = 8,
        X,
        Square,
        Circle,
        R1,
        R2,
        R3, // Not properly supported, gives broken graphics on the settings screen
        RightStickLeft = 0xA0,
        RightStickUp,
        LeftStickLeft,
        LeftStickUp,
        RightStickRight = 0xC0,
        RightStickDown,
        LeftStickRight,
        LeftStickDown
    }
}