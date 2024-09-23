using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Settings.Controller
{
    public class UnknownControllerSettings
    {
        public UnknownSteeringButtonEnum SteerLeftButton { get; set; }
        public UnknownSteeringButtonEnum SteerRightButton { get; set; }
        public ControllerButtonEnum AccelerateButton { get; set; }
        public ControllerButtonEnum BrakeButton { get; set; }
        public ControllerButtonEnum HandbrakeButton { get; set; }
        public ControllerButtonEnum ReverseButton { get; set; }
        public ControllerButtonEnum ShiftUpButton { get; set; }
        public ControllerButtonEnum ShiftDownButton { get; set; }
        public ControllerButtonEnum ChangeViewsButton { get; set; }
        public ControllerButtonEnum RearViewButton { get; set; }
        public ushort UnknownDeadzone { get; set; }
        public ushort UnknownLimit { get; set; }

        public void ReadBindingsFromSave(Stream file)
        {
            SteerLeftButton = (UnknownSteeringButtonEnum)file.ReadSingleByte();
            SteerRightButton = (UnknownSteeringButtonEnum)file.ReadSingleByte();
            AccelerateButton = (ControllerButtonEnum)file.ReadSingleByte();
            BrakeButton = (ControllerButtonEnum)file.ReadSingleByte();
            HandbrakeButton = (ControllerButtonEnum)file.ReadSingleByte();
            ReverseButton = (ControllerButtonEnum)file.ReadSingleByte();
            ShiftUpButton = (ControllerButtonEnum)file.ReadSingleByte();
            ShiftDownButton = (ControllerButtonEnum)file.ReadSingleByte();
            ChangeViewsButton = (ControllerButtonEnum)file.ReadSingleByte();
            RearViewButton = (ControllerButtonEnum)file.ReadSingleByte();
        }

        public void ReadSettingsFromSave(Stream file)
        {
            UnknownDeadzone = file.ReadUShort();
            UnknownLimit = file.ReadUShort();
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
        }

        public void WriteSettingsToSave(Stream file)
        {
            file.WriteUShort(UnknownDeadzone);
            file.WriteUShort(UnknownLimit);
        }
    }
}