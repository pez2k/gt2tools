namespace GT2.DataSplitter.Models.Common
{
    public class Gear
    {
        public string CarId { get; set; } = "";
        public uint Price { get; set; }
        public byte Stage { get; set; }
        public byte NumberOfGears { get; set; }
        public short ReverseGearRatio { get; set; }
        public short FirstGearRatio { get; set; }
        public short SecondGearRatio { get; set; }
        public short ThirdGearRatio { get; set; }
        public short FourthGearRatio { get; set; }
        public short FifthGearRatio { get; set; }
        public short SixthGearRatio { get; set; }
        public short SeventhGearRatio { get; set; }
        public short DefaultFinalDriveRatio { get; set; }
        public short MaxFinalDriveRatio { get; set; }
        public short MinFinalDriveRatio { get; set; }
        public bool AllowIndividualRatioAdjustments { get; set; }
        public byte DefaultAutoSetting { get; set; }
        public byte MinAutoSetting { get; set; }
        public byte MaxAutoSetting { get; set; }
    }
}