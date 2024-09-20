using System.IO;
using StreamExtensions;

namespace GT2.SaveEditor
{
    public class DigitalControllerSettings
    {
        public ControllerButtonEnum SteerLeftButton { get; set; }
        public ControllerButtonEnum SteerRightButton { get; set; }
        public ControllerButtonEnum AccelerateButton { get; set; }
        public ControllerButtonEnum BrakeButton { get; set; }
        public ControllerButtonEnum HandbrakeButton { get; set; }
        public ControllerButtonEnum ReverseButton { get; set; }
        public ControllerButtonEnum ShiftUpButton { get; set; }
        public ControllerButtonEnum ShiftDownButton { get; set; }
        public ControllerButtonEnum ChangeViewsButton { get; set; }
        public ControllerButtonEnum RearViewButton { get; set;  }

        public void ReadFromSave(Stream file)
        {
            SteerLeftButton = (ControllerButtonEnum)file.ReadSingleByte();
            SteerRightButton = (ControllerButtonEnum)file.ReadSingleByte();
            AccelerateButton = (ControllerButtonEnum)file.ReadSingleByte();
            BrakeButton = (ControllerButtonEnum)file.ReadSingleByte();
            HandbrakeButton = (ControllerButtonEnum)file.ReadSingleByte();
            ReverseButton = (ControllerButtonEnum)file.ReadSingleByte();
            ShiftUpButton = (ControllerButtonEnum)file.ReadSingleByte();
            ShiftDownButton = (ControllerButtonEnum)file.ReadSingleByte();
            ChangeViewsButton = (ControllerButtonEnum)file.ReadSingleByte();
            RearViewButton = (ControllerButtonEnum)file.ReadSingleByte();
        }
    }
}