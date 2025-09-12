using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Gear : MappedDataStructure<Gear.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x24
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte NumberOfGears;
            public short ReverseGearRatio;
            public short FirstGearRatio;
            public short SecondGearRatio;
            public short ThirdGearRatio;
            public short FourthGearRatio;
            public short FifthGearRatio;
            public short SixthGearRatio;
            public short SeventhGearRatio;
            public short DefaultFinalDriveRatio;
            public short MaxFinalDriveRatio;
            public short MinFinalDriveRatio;
            public byte AllowIndividualRatioAdjustments;
            public byte DefaultAutoSetting;
            public byte MinAutoSetting;
            public byte MaxAutoSetting;
        }

        public Models.Common.Gear MapToModel() =>
            new Models.Common.Gear
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                NumberOfGears = data.NumberOfGears,
                ReverseGearRatio = data.ReverseGearRatio,
                FirstGearRatio = data.FirstGearRatio,
                SecondGearRatio = data.SecondGearRatio,
                ThirdGearRatio = data.ThirdGearRatio,
                FourthGearRatio = data.FourthGearRatio,
                FifthGearRatio = data.FifthGearRatio,
                SixthGearRatio = data.SixthGearRatio,
                SeventhGearRatio = data.SeventhGearRatio,
                DefaultFinalDriveRatio = data.DefaultFinalDriveRatio,
                MaxFinalDriveRatio = data.MaxFinalDriveRatio,
                MinFinalDriveRatio = data.MinFinalDriveRatio,
                AllowIndividualRatioAdjustments = data.AllowIndividualRatioAdjustments == 1,
                DefaultAutoSetting = data.DefaultAutoSetting,
                MinAutoSetting = data.MinAutoSetting,
                MaxAutoSetting = data.MaxAutoSetting
            };
    }
}