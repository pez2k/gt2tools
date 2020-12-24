using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class Steer : CarCsvDataStructure<SteerData, SteerCSVMap>
    {
        public Steer()
        {
            HasCarId = false;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x18
    public struct SteerData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte Unknown1;
        public byte Angle1Speed;
        public byte Angle2Speed;
        public byte Angle3Speed;
        public byte Angle4Speed;
        public byte Angle5Speed;
        public byte Angle6Speed;
        public byte Angle1;
        public byte Angle2;
        public byte Angle3;
        public byte Angle4;
        public byte Angle5;
        public byte Angle6;
        public byte MaxSteeringAngle;
        public byte Unknown2;
    }

    public sealed class SteerCSVMap : ClassMap<SteerData>
    {
        public SteerCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.Unknown1);
            Map(m => m.Angle1Speed);
            Map(m => m.Angle2Speed);
            Map(m => m.Angle3Speed);
            Map(m => m.Angle4Speed);
            Map(m => m.Angle5Speed);
            Map(m => m.Angle6Speed);
            Map(m => m.Angle1);
            Map(m => m.Angle2);
            Map(m => m.Angle3);
            Map(m => m.Angle4);
            Map(m => m.Angle5);
            Map(m => m.Angle6);
            Map(m => m.MaxSteeringAngle);
            Map(m => m.Unknown2);
        }
    }
}
