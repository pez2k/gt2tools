using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class Engine : CarCsvDataStructure<EngineData, EngineCSVMap>
    {
        protected override string CreateOutputFilename() => Name + "\\" + data.CarId.ToCarName() + ".csv";
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x4C
    public struct EngineData
    {
        public uint CarId; // 0
        public ushort LayoutName; // V6 etc, index into unistringdb, 0x4
        public ushort ValvetrainName; // DOHC etc, index into unistringdb, 0x6
        public ushort Aspiration; // 
        public ushort SoundFile; // 
        public ushort TorqueCurve1; // 0xc - all these take part in garage HP calculation
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
        public ushort Displacement; // 0x2c - not quite for those that have numberx2 displacements - TODO: handle multiplier bits
        public ushort DisplayedPower; // 0x2e - base ps. F. Ex. is 295hp for most of the rally cars, not 400+
        public ushort MaxPowerRPM; // 0x30 - multipled by 10 by the game. E.g. a value of 850 is displayed as 8500
        public ushort DisplayedTorque; // 0x32 - divided by 10 by the game. e.g. a value of 950 is displayed as 95.0
        public ushort MaxTorqueRPMName; // 0x34 - index into unistringdb!
        public byte PowerMultiplier; // 0x36 - used in hp calculation
        public byte ClutchReleaseRPM;
        public byte IdleRPM;
        public byte MaxRPM;
        public byte RedlineRPM;
        public byte TorqueCurveRPM1; // 0x3b - rpms that the bandAcceleration values are for
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
        public byte TorqueCurvePoints; // the number of values in both arrays that are used in hp calculations
    }

    public sealed class EngineCSVMap : ClassMap<EngineData>
    {
        public EngineCSVMap()
        {
            Map(m => m.CarId).CarId();
            Map(m => m.LayoutName).TypeConverter(StringTable.Lookup);
            Map(m => m.ValvetrainName).TypeConverter(StringTable.Lookup);
            Map(m => m.Aspiration).TypeConverter(StringTable.Lookup);
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
            Map(m => m.Displacement);
            Map(m => m.DisplayedPower);
            Map(m => m.MaxPowerRPM);
            Map(m => m.DisplayedTorque);
            Map(m => m.MaxTorqueRPMName).TypeConverter(StringTable.Lookup);
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