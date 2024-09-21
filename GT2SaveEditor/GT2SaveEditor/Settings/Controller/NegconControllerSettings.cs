using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor.Settings.Controller
{
    public class NegconControllerSettings
    {
        public ushort SteeringLimitLeft { get; set; }   // Range is 0 to 256, default limit leaves 64 gap either side, so 64 to 192 as limits
        public ushort SteeringCentreLeft { get; set; }  // Deadzone moves the centre away from the centre of the axis (128), default is 16 so 120 left and 136 right
        public ushort SteeringCentreRight { get; set; } // Moving centre point simply adds that value to all four points
        public ushort SteeringLimitRight { get; set; }
        public ushort AccelerationDeadzone { get; set; }
        public ushort AccelerationLimit { get; set; }
        public ushort BrakeDeadzone { get; set; }
        public ushort BrakeLimit { get; set; }

        public void ReadFromSave(Stream file)
        {
            SteeringLimitLeft = file.ReadUShort();
            SteeringCentreLeft = file.ReadUShort();
            SteeringCentreRight = file.ReadUShort();
            SteeringLimitRight = file.ReadUShort();
            AccelerationDeadzone = file.ReadUShort();
            AccelerationLimit = file.ReadUShort();
            BrakeDeadzone = file.ReadUShort();
            BrakeLimit = file.ReadUShort();
        }

        public void WriteToSave(Stream file)
        {
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