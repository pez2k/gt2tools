using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class Drivetrain : CarCsvDataStructure<DrivetrainData, DrivetrainCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Data.CarId.ToCarName() + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
    public struct DrivetrainData
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
        public byte Unknown5;
        public byte Unknown6;
    }

    public sealed class DrivetrainCSVMap : ClassMap<DrivetrainData>
    {
        public DrivetrainCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Unknown);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.DrivetrainType).TypeConverter(Utils.DrivetrainTypeConverter);
            Map(m => m.AWDBehaviour);
            Map(m => m.DefaultClutchRPMDropRate);
            Map(m => m.DefaultClutchInertiaEngaged);
            Map(m => m.DefaultClutchInertialWeight);
            Map(m => m.DefaultClutchInertiaDisengaged);
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
        }
    }
}
