using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Settings.Controller
{
    public class NegconControllerSettings
    {
        public NegconSteeringButtonEnum SteerLeftButton { get; set; }
        public NegconSteeringButtonEnum SteerRightButton { get; set; }
        public NegconAccelerateBrakeButtonEnum AccelerateButton { get; set; }
        public NegconAccelerateBrakeButtonEnum BrakeButton { get; set; }
        public NegconButtonEnum HandbrakeButton { get; set; }
        public NegconButtonEnum ReverseButton { get; set; }
        public NegconButtonEnum ShiftUpButton { get; set; }
        public NegconButtonEnum ShiftDownButton { get; set; }
        public NegconButtonEnum ChangeViewsButton { get; set; }
        public NegconButtonEnum RearViewButton { get; set; }
        public NegconSteeringModeEnum SteeringMode { get; set; }
        public ushort SteeringLimitLeft { get; set; }   // Range is 0 to 256, default limit leaves 64 gap either side, so 64 to 192 as limits
        public ushort SteeringCentreLeft { get; set; }  // Deadzone moves the centre away from the centre of the axis (128), default is 16 so 120 left and 136 right
        public ushort SteeringCentreRight { get; set; } // Moving centre point simply adds that value to all four points
        public ushort SteeringLimitRight { get; set; }
        public ushort AccelerationDeadzone { get; set; }
        public ushort AccelerationLimit { get; set; }
        public ushort BrakeDeadzone { get; set; }
        public ushort BrakeLimit { get; set; }

        public void ReadBindingsFromSave(Stream file)
        {
            SteerLeftButton = (NegconSteeringButtonEnum)file.ReadSingleByte();
            SteerRightButton = (NegconSteeringButtonEnum)file.ReadSingleByte();
            AccelerateButton = (NegconAccelerateBrakeButtonEnum)file.ReadSingleByte();
            BrakeButton = (NegconAccelerateBrakeButtonEnum)file.ReadSingleByte();
            HandbrakeButton = (NegconButtonEnum)file.ReadSingleByte();
            ReverseButton = (NegconButtonEnum)file.ReadSingleByte();
            ShiftUpButton = (NegconButtonEnum)file.ReadSingleByte();
            ShiftDownButton = (NegconButtonEnum)file.ReadSingleByte();
            ChangeViewsButton = (NegconButtonEnum)file.ReadSingleByte();
            RearViewButton = (NegconButtonEnum)file.ReadSingleByte();
            file.Position += 0x1;
        }

        public void ReadSettingsFromSave(Stream file)
        {
            SteeringMode = (NegconSteeringModeEnum)file.ReadSingleByte();
            file.Position += 0x8;
            SteeringLimitLeft = file.ReadUShort();
            SteeringCentreLeft = file.ReadUShort();
            SteeringCentreRight = file.ReadUShort();
            SteeringLimitRight = file.ReadUShort();
            AccelerationDeadzone = file.ReadUShort();
            AccelerationLimit = file.ReadUShort();
            BrakeDeadzone = file.ReadUShort();
            BrakeLimit = file.ReadUShort();
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
            file.WriteByte((byte)SteeringMode);
            file.Position += 0x8;
            file.WriteUShort(SteeringLimitLeft);
            file.WriteUShort(SteeringCentreLeft);
            file.WriteUShort(SteeringCentreRight);
            file.WriteUShort(SteeringLimitRight);
            file.WriteUShort(AccelerationDeadzone);
            file.WriteUShort(AccelerationLimit);
            file.WriteUShort(BrakeDeadzone);
            file.WriteUShort(BrakeLimit);
        }
    }
}