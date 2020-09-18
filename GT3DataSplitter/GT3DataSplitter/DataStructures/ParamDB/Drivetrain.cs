using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Drivetrain : CsvDataStructure<DrivetrainData, DrivetrainCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x28
    public struct DrivetrainData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte TorqueSplitMin;
        public byte TorqueSplitMax;
        public byte TorqueSplitDefault;
        public byte DrivetrainType;
        public byte AWDBehaviour;
        public ushort EngineBraking;
        public ushort FrontWheelInertia;
        public ushort RearWheelInertia;
        public ushort FrontPropshaftInertia;
        public ushort RearPropshaftInertia;
        public ushort FlywheelInertia;
        public ushort Padding;
        public uint Price;
    }

    public sealed class DrivetrainCSVMap : ClassMap<DrivetrainData>
    {
        public DrivetrainCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.TorqueSplitMin);
            Map(m => m.TorqueSplitMax);
            Map(m => m.TorqueSplitDefault);
            Map(m => m.DrivetrainType);
            Map(m => m.AWDBehaviour);
            Map(m => m.EngineBraking);
            Map(m => m.FrontWheelInertia);
            Map(m => m.RearWheelInertia);
            Map(m => m.FrontPropshaftInertia);
            Map(m => m.RearPropshaftInertia);
            Map(m => m.FlywheelInertia);
            Map(m => m.Price);
        }
    }
}