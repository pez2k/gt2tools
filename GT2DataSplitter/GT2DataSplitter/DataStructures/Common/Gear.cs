using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using TypeConverters;

    public class Gear : CarCsvDataStructure<GearData, GearCSVMap>
    {
        protected override string CreateOutputFilename() => CreateOutputFilename(data.CarId, data.Stage);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x24
    public struct GearData
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

    public sealed class GearCSVMap : ClassMap<GearData>
    {
        public GearCSVMap()
        {
            Map(m => m.CarId).CarId();
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.NumberOfGears);
            Map(m => m.ReverseGearRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.FirstGearRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.SecondGearRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.ThirdGearRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.FourthGearRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.FifthGearRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.SixthGearRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.SeventhGearRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.DefaultFinalDriveRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.MaxFinalDriveRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.MinFinalDriveRatio).TypeConverter<BackwardCompatibleShortConverter>();
            Map(m => m.AllowIndividualRatioAdjustments);
            Map(m => m.DefaultAutoSetting);
            Map(m => m.MinAutoSetting);
            Map(m => m.MaxAutoSetting);
        }
    }
}