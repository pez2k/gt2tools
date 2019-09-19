using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Engine : CsvDataStructure<EngineData, EngineCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x58
    public struct EngineData
    {
        public ulong Part;
        public ulong Car;
        public ushort LayoutName;
        public ushort ValvetrainName;
        public ushort Aspiration;
        public ushort SoundFile;
        public ushort TorqueCurve1;
        public ushort TorqueCurve2;
        public ushort TorqueCurve3;
        public ushort TorqueCurve4;
        public ushort TorqueCurve5;
        public ushort TorqueCurve6;
        public ushort TorqueCurve7;
        public ushort TorqueCurve8;
        public ushort TorqueCurve9;
        public ushort TorqueCurve10;
        public ushort TorqueCurve11;
        public ushort TorqueCurve12;
        public ushort TorqueCurve13;
        public ushort TorqueCurve14;
        public ushort TorqueCurve15;
        public ushort TorqueCurve16;
        public ushort Displacement;
        public ushort DisplayedPower;
        public ushort MaxPowerRPM;
        public ushort DisplayedTorque;
        public ushort MaxTorqueRPM;
        public byte PowerMultiplier;
        public byte ClutchReleaseRPM;
        public byte IdleRPM;
        public byte MaxRPM;
        public byte RedlineRPM;
        public byte TorqueCurveRPM1;
        public byte TorqueCurveRPM2;
        public byte TorqueCurveRPM3;
        public byte TorqueCurveRPM4;
        public byte TorqueCurveRPM5;
        public byte TorqueCurveRPM6;
        public byte TorqueCurveRPM7;
        public byte TorqueCurveRPM8;
        public byte TorqueCurveRPM9;
        public byte TorqueCurveRPM10;
        public byte TorqueCurveRPM11;
        public byte TorqueCurveRPM12;
        public byte TorqueCurveRPM13;
        public byte TorqueCurveRPM14;
        public byte TorqueCurveRPM15;
        public byte TorqueCurveRPM16;
        public byte TorqueCurvePoints;
    }

    public sealed class EngineCSVMap : ClassMap<EngineData>
    {
        public EngineCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.LayoutName).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.ValvetrainName).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.Aspiration).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.SoundFile);
            Map(m => m.TorqueCurve1);
            Map(m => m.TorqueCurve2);
            Map(m => m.TorqueCurve3);
            Map(m => m.TorqueCurve4);
            Map(m => m.TorqueCurve5);
            Map(m => m.TorqueCurve6);
            Map(m => m.TorqueCurve7);
            Map(m => m.TorqueCurve8);
            Map(m => m.TorqueCurve9);
            Map(m => m.TorqueCurve10);
            Map(m => m.TorqueCurve11);
            Map(m => m.TorqueCurve12);
            Map(m => m.TorqueCurve13);
            Map(m => m.TorqueCurve14);
            Map(m => m.TorqueCurve15);
            Map(m => m.TorqueCurve16);
            Map(m => m.Displacement).TypeConverter(Program.Strings.Lookup);
            Map(m => m.DisplayedPower);
            Map(m => m.MaxPowerRPM).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.DisplayedTorque);
            Map(m => m.MaxTorqueRPM).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.PowerMultiplier);
            Map(m => m.ClutchReleaseRPM);
            Map(m => m.IdleRPM);
            Map(m => m.MaxRPM);
            Map(m => m.RedlineRPM);
            Map(m => m.TorqueCurveRPM1);
            Map(m => m.TorqueCurveRPM2);
            Map(m => m.TorqueCurveRPM3);
            Map(m => m.TorqueCurveRPM4);
            Map(m => m.TorqueCurveRPM5);
            Map(m => m.TorqueCurveRPM6);
            Map(m => m.TorqueCurveRPM7);
            Map(m => m.TorqueCurveRPM8);
            Map(m => m.TorqueCurveRPM9);
            Map(m => m.TorqueCurveRPM10);
            Map(m => m.TorqueCurveRPM11);
            Map(m => m.TorqueCurveRPM12);
            Map(m => m.TorqueCurveRPM13);
            Map(m => m.TorqueCurveRPM14);
            Map(m => m.TorqueCurveRPM15);
            Map(m => m.TorqueCurveRPM16);
            Map(m => m.TorqueCurvePoints);
        }
    }
}