using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public abstract class Equip : CsvDataStructure<EquipData, EquipCSVMap>
    {
        public Equip() => Header = "EQUIP";

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{Encoding.ASCII.GetString(data.Name).TrimEnd('\0')}{Path.GetExtension(filename)}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x6C
    public struct EquipData
    {
        public ushort CarID;
        public ushort AdjustMaybe;
        public ushort EngineBalancing;
        public ushort Brake;
        public ushort BrakeController;
        public ushort Clutch;
        public ushort Compressor;
        public ushort Computer;
        public ushort Displacement;
        public ushort Flywheel;
        public ushort Gear;
        public ushort Intercooler;
        public ushort Lightweight;
        public ushort Muffler;
        public ushort NATune;
        public ushort PortPolish;
        public ushort Propshaft;
        public ushort RacingModify;
        public ushort StabilizerFront;
        public ushort StabilizerRear;
        public ushort Suspension;
        public ushort Tire;
        public ushort TurbineKit;
        public ushort FirstGearRatio;
        public ushort SecondGearRatio;
        public ushort ThirdGearRatio;
        public ushort FourthGearRatio;
        public ushort FifthGearRatio;
        public ushort SixthGearRatio;
        public ushort SeventhGearRatio;
        public ushort FinalDriveRatio;
        public ushort Unknown;
        public byte FrontUnknown;
        public byte FrontDownforceMaybe;
        public byte FrontUnknown2;
        public byte FrontCamber;
        public byte FrontRideHeight;
        public byte FrontSpringRate;
        public byte FrontDamperBound;
        public byte FrontDamperRebound;
        public byte FrontDamperBound2Maybe;
        public byte FrontDamperRebound2Maybe;
        public byte FrontUnknown3;
        public byte FrontUnknown4;
        public byte FrontUnknown5;
        public byte RearUnknown;
        public byte RearDownforceMaybe;
        public byte RearUnknown2;
        public byte RearCamber;
        public byte RearRideHeight;
        public byte RearSpringRate;
        public byte RearDamperUnknown;
        public byte RearDamperUnknown2;
        public byte RearDamperUnknown3;
        public byte RearDamperUnknown4;
        public byte RearUnknown3;
        public byte RearUnknown4;
        public byte RearUnknown5;
        public ushort Padding8;
        public ushort Padding9;
        public byte Padding10;
        public byte Unknown2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] Name;
    }

    public sealed class EquipCSVMap : ClassMap<EquipData>
    {
        public EquipCSVMap()
        {
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.AdjustMaybe).PartFilename(nameof(Adjust));
            Map(m => m.EngineBalancing).PartFilename(nameof(EngineBalancing));
            Map(m => m.Brake).PartFilename(nameof(Brake));
            Map(m => m.BrakeController).PartFilename(nameof(BrakeController));
            Map(m => m.Clutch).PartFilename(nameof(Clutch));
            Map(m => m.Compressor).PartFilename(nameof(Compressor));
            Map(m => m.Computer).PartFilename(nameof(Computer));
            Map(m => m.Displacement).PartFilename(nameof(Displacement));
            Map(m => m.Flywheel).PartFilename(nameof(Flywheel));
            Map(m => m.Gear).PartFilename(nameof(Gear));
            Map(m => m.Intercooler).PartFilename(nameof(Intercooler));
            Map(m => m.Lightweight).PartFilename(nameof(Lightweight));
            Map(m => m.Muffler).PartFilename(nameof(Muffler));
            Map(m => m.NATune).PartFilename(nameof(NATune));
            Map(m => m.PortPolish).PartFilename(nameof(PortPolish));
            Map(m => m.Propshaft).PartFilename(nameof(Propshaft));
            Map(m => m.RacingModify).PartFilename(nameof(RacingModify));
            Map(m => m.StabilizerFront).PartFilename(nameof(StabilizerFront));
            Map(m => m.StabilizerRear).PartFilename(nameof(StabilizerRear));
            Map(m => m.Suspension).PartFilename(nameof(Suspension));
            Map(m => m.Tire).PartFilename(nameof(Tire));
            Map(m => m.TurbineKit).PartFilename(nameof(TurbineKit));
            Map(m => m.FirstGearRatio);
            Map(m => m.SecondGearRatio);
            Map(m => m.ThirdGearRatio);
            Map(m => m.FourthGearRatio);
            Map(m => m.FifthGearRatio);
            Map(m => m.SixthGearRatio);
            Map(m => m.SeventhGearRatio);
            Map(m => m.FinalDriveRatio);
            Map(m => m.Unknown);
            Map(m => m.FrontUnknown);
            Map(m => m.FrontDownforceMaybe);
            Map(m => m.FrontUnknown2);
            Map(m => m.FrontCamber);
            Map(m => m.FrontRideHeight);
            Map(m => m.FrontSpringRate);
            Map(m => m.FrontDamperBound);
            Map(m => m.FrontDamperRebound);
            Map(m => m.FrontDamperBound2Maybe);
            Map(m => m.FrontDamperRebound2Maybe);
            Map(m => m.FrontUnknown3);
            Map(m => m.FrontUnknown4);
            Map(m => m.FrontUnknown5);
            Map(m => m.RearUnknown);
            Map(m => m.RearDownforceMaybe);
            Map(m => m.RearUnknown2);
            Map(m => m.RearCamber);
            Map(m => m.RearRideHeight);
            Map(m => m.RearSpringRate);
            Map(m => m.RearDamperUnknown);
            Map(m => m.RearDamperUnknown2);
            Map(m => m.RearDamperUnknown3);
            Map(m => m.RearDamperUnknown4);
            Map(m => m.RearUnknown3);
            Map(m => m.RearUnknown4);
            Map(m => m.RearUnknown5);
            Map(m => m.Unknown2);
            Map(m => m.Name).TypeConverter(new HexStringConverter(12));
        }
    }
}