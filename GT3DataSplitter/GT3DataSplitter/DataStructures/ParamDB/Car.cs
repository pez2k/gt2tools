using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Car : CsvDataStructure<CarData, CarCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x108
    public struct CarData
    {
        public ulong Car;
        public ulong Brake;
        public ulong BrakeController;
        public ulong Steer;
        public ulong Chassis;
        public ulong Lightweight;
        public ulong RacingModify;
        public ulong Engine;
        public ulong PortPolish;
        public ulong EngineBalance;
        public ulong Displacement;
        public ulong Computer;
        public ulong NATune;
        public ulong TurbineKit;
        public ulong Drivetrain;
        public ulong Flywheel;
        public ulong Clutch;
        public ulong PropellerShaft;
        public ulong LSD;
        public ulong Gear;
        public ulong Suspension;
        public ulong Intercooler;
        public ulong Muffler;
        public ulong FrontTire;
        public ulong RearTire;
        public ulong ASCC;
        public ulong TCSC;
        public ulong WheelsMaybe;
        public ushort Unknown1;
        public ushort ManufacturerID;
        public ushort NameFirstPartMaybe;
        public ushort NameSecondPartMaybe;
        public ushort Unknown2;
        public ushort Year;
        public uint Price;
        public byte IsSpecialMaybe;
        public byte Unknown3;
        public ushort Unknown4;
        public ushort Unknown5;
        public ushort Unknown6;
        public ulong Unknown7;
        public ushort Unknown8;
        public ushort Unknown9;
        public ushort Unknown10;
        public ushort Unknown11;
    }

    public sealed class CarCSVMap : ClassMap<CarData>
    {
        public CarCSVMap()
        {
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Brake).TypeConverter(Utils.IdConverter);
            Map(m => m.BrakeController).TypeConverter(Utils.IdConverter);
            Map(m => m.Steer).TypeConverter(Utils.IdConverter);
            Map(m => m.Chassis).TypeConverter(Utils.IdConverter);
            Map(m => m.Lightweight).TypeConverter(Utils.IdConverter);
            Map(m => m.RacingModify).TypeConverter(Utils.IdConverter);
            Map(m => m.Engine).TypeConverter(Utils.IdConverter);
            Map(m => m.PortPolish).TypeConverter(Utils.IdConverter);
            Map(m => m.EngineBalance).TypeConverter(Utils.IdConverter);
            Map(m => m.Displacement).TypeConverter(Utils.IdConverter);
            Map(m => m.Computer).TypeConverter(Utils.IdConverter);
            Map(m => m.NATune).TypeConverter(Utils.IdConverter);
            Map(m => m.TurbineKit).TypeConverter(Utils.IdConverter);
            Map(m => m.Drivetrain).TypeConverter(Utils.IdConverter);
            Map(m => m.Flywheel).TypeConverter(Utils.IdConverter);
            Map(m => m.Clutch).TypeConverter(Utils.IdConverter);
            Map(m => m.PropellerShaft).TypeConverter(Utils.IdConverter);
            Map(m => m.LSD).TypeConverter(Utils.IdConverter);
            Map(m => m.Gear).TypeConverter(Utils.IdConverter);
            Map(m => m.Suspension).TypeConverter(Utils.IdConverter);
            Map(m => m.Intercooler).TypeConverter(Utils.IdConverter);
            Map(m => m.Muffler).TypeConverter(Utils.IdConverter);
            Map(m => m.FrontTire).TypeConverter(Utils.IdConverter);
            Map(m => m.RearTire).TypeConverter(Utils.IdConverter);
            Map(m => m.ASCC).TypeConverter(Utils.IdConverter);
            Map(m => m.TCSC).TypeConverter(Utils.IdConverter);
            Map(m => m.WheelsMaybe);
            Map(m => m.Unknown1);
            Map(m => m.ManufacturerID);
            Map(m => m.NameFirstPartMaybe).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.NameSecondPartMaybe).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.Unknown2);
            Map(m => m.Year);
            Map(m => m.Price);
            Map(m => m.IsSpecialMaybe);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.Unknown10);
            Map(m => m.Unknown11);
        }
    }
}