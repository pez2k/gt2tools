using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Settings.Controller
{
    public class AnalogueControllerSettings
    {
        public AnalogueSteeringButtonEnum SteerLeftButton { get; set; }
        public AnalogueSteeringButtonEnum SteerRightButton { get; set; }
        public AnalogueAccelerateBrakeButtonEnum AccelerateButton { get; set; }
        public AnalogueAccelerateBrakeButtonEnum BrakeButton { get; set; }
        public ControllerButtonEnum HandbrakeButton { get; set; }
        public ControllerButtonEnum ReverseButton { get; set; }
        public ControllerButtonEnum ShiftUpButton { get; set; }
        public ControllerButtonEnum ShiftDownButton { get; set; }
        public ControllerButtonEnum ChangeViewsButton { get; set; }
        public ControllerButtonEnum RearViewButton { get; set; }
        public bool Unknown { get; set; }
        public bool VibrationEnabled { get; set; }
        public AnalogueSteeringModeEnum SteeringMode { get; set; }
        public AnalogueAccelerateBrakeModeEnum AccelerateBrakeMode { get; set; }
        public ControllerButtonEnum LastDigitalAccelerateButton { get; set; }
        public ControllerButtonEnum LastDigitalBrakeButton { get; set; }

        public void ReadBindingsFromSave(Stream file)
        {
            SteerLeftButton = (AnalogueSteeringButtonEnum)file.ReadSingleByte();
            SteerRightButton = (AnalogueSteeringButtonEnum)file.ReadSingleByte();
            AccelerateButton = (AnalogueAccelerateBrakeButtonEnum)file.ReadSingleByte();
            BrakeButton = (AnalogueAccelerateBrakeButtonEnum)file.ReadSingleByte();
            HandbrakeButton = (ControllerButtonEnum)file.ReadSingleByte();
            ReverseButton = (ControllerButtonEnum)file.ReadSingleByte();
            ShiftUpButton = (ControllerButtonEnum)file.ReadSingleByte();
            ShiftDownButton = (ControllerButtonEnum)file.ReadSingleByte();
            ChangeViewsButton = (ControllerButtonEnum)file.ReadSingleByte();
            RearViewButton = (ControllerButtonEnum)file.ReadSingleByte();
            file.Position += 0x1;
        }

        public void ReadSettingsFromSave(Stream file)
        {
            Unknown = file.ReadByteAsBool();
            VibrationEnabled = !file.ReadByteAsBool();
            file.Position += 0x4;
            SteeringMode = (AnalogueSteeringModeEnum)file.ReadSingleByte();
            AccelerateBrakeMode = (AnalogueAccelerateBrakeModeEnum)file.ReadSingleByte();
            LastDigitalAccelerateButton = (ControllerButtonEnum)file.ReadSingleByte();
            LastDigitalBrakeButton = (ControllerButtonEnum)file.ReadSingleByte();
        }

        public void WriteBindingsToSave(Stream file)
        {
            file.WriteByte((byte)SteerLeftButton);
            file.WriteByte((byte)SteerRightButton);
            file.WriteByte((byte)AccelerateButton);
            file.WriteByte((byte)BrakeButton);
            file.WriteByte((byte)HandbrakeButton);
            file.WriteByte((byte)ReverseButton);
            file.WriteByte((byte)ShiftUpButton);
            file.WriteByte((byte)ShiftDownButton);
            file.WriteByte((byte)ChangeViewsButton);
            file.WriteByte((byte)RearViewButton);
            file.Position += 0x1;
        }

        public void WriteSettingsToSave(Stream file)
        {
            file.WriteBoolAsByte(Unknown);
            file.WriteBoolAsByte(!VibrationEnabled);
            file.Position += 0x4;
            file.WriteByte((byte)SteeringMode);
            file.WriteByte((byte)AccelerateBrakeMode);
            file.WriteByte((byte)LastDigitalAccelerateButton);
            file.WriteByte((byte)LastDigitalBrakeButton);
        }
    }
}