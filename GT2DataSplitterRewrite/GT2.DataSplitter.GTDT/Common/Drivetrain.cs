using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;
    using Models.Enums;

    public class Drivetrain : MappedDataStructure<Drivetrain.Data, Models.Common.Drivetrain>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
        public struct Data
        {
            public uint CarId;
            public byte Unknown;
            public byte Unknown2;
            public byte Unknown3;
            public byte Unknown4;
            public byte DrivetrainType;
            public byte AWDBehaviour;
            public byte DefaultClutchRPMDropRate;
            public byte DefaultClutchInertiaEngaged;
            public byte DefaultClutchInertialWeight;
            public byte DefaultClutchInertiaDisengaged;
            public byte FrontDriveInertia;
            public byte RearDriveInertia;
        }

        public override Models.Common.Drivetrain MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.Drivetrain
            {
                CarId = data.CarId.ToCarName(),
                Unknown = data.Unknown,
                Unknown2 = data.Unknown2,
                Unknown3 = data.Unknown3,
                Unknown4 = data.Unknown4,
                DrivetrainType = (DrivetrainType)data.DrivetrainType,
                AWDBehaviour = data.AWDBehaviour,
                DefaultClutchRPMDropRate = data.DefaultClutchRPMDropRate,
                DefaultClutchInertiaEngaged = data.DefaultClutchInertiaEngaged,
                DefaultClutchInertialWeight = data.DefaultClutchInertialWeight,
                DefaultClutchInertiaDisengaged = data.DefaultClutchInertiaDisengaged,
                FrontDriveInertia = data.FrontDriveInertia,
                RearDriveInertia = data.RearDriveInertia
            };
    }
}