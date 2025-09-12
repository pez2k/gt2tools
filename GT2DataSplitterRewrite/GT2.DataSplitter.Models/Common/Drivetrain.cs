using GT2.DataSplitter.Models.Enums;

namespace GT2.DataSplitter.Models.Common
{
    public class Drivetrain
    {
        public string CarId { get; set; } = "";
        public byte Unknown { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }
        public byte Unknown4 { get; set; }
        public DrivetrainType DrivetrainType { get; set; }
        public byte AWDBehaviour { get; set; }
        public byte DefaultClutchRPMDropRate { get; set; }
        public byte DefaultClutchInertiaEngaged { get; set; }
        public byte DefaultClutchInertialWeight { get; set; }
        public byte DefaultClutchInertiaDisengaged { get; set; }
        public byte FrontDriveInertia { get; set; }
        public byte RearDriveInertia { get; set; }
    }
}