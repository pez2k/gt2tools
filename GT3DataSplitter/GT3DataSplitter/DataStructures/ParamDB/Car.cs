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
        public ushort TunerIDMaybe;
        public ushort ManufacturerID;
        public ushort NameFirstPartUS;
        public ushort NameSecondPartUS;
        public ushort Label;
        public ushort Year;
        public uint Price;
        public byte IsSpecialMaybe;
        public byte Unknown;
        public ushort NameFirstPartJP;
        public ushort NameSecondPartJP;
        public ushort Unknown2;
        public ulong Unknown3;
        public ushort NameFirstPartEU;
        public ushort NameSecondPartEU;
        public ushort ManufacturerName;
        public ushort Unknown4;
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
            Map(m => m.TunerIDMaybe);
            Map(m => m.ManufacturerID);
            Map(m => m.NameFirstPartUS).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.NameSecondPartUS).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.Label).TypeConverter(Program.Strings.Lookup);
            Map(m => m.Year);
            Map(m => m.Price);
            Map(m => m.IsSpecialMaybe);
            Map(m => m.Unknown);
            Map(m => m.NameFirstPartJP).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.NameSecondPartJP).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.NameFirstPartEU).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.NameSecondPartEU).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.ManufacturerName).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.Unknown4);
        }
    }
}