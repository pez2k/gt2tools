using System.Globalization;
using System.Runtime.InteropServices;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter
{
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
            Map(m => m.ReverseGearRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.FirstGearRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.SecondGearRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.ThirdGearRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.FourthGearRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.FifthGearRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.SixthGearRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.SeventhGearRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.DefaultFinalDriveRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.MaxFinalDriveRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.MinFinalDriveRatio).TypeConverter<BackwardCompatibleRatioConverter>();
            Map(m => m.AllowIndividualRatioAdjustments);
            Map(m => m.DefaultAutoSetting);
            Map(m => m.MinAutoSetting);
            Map(m => m.MaxAutoSetting);
        }

        private class BackwardCompatibleRatioConverter : Int16Converter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                NumberStyles numberStyle = memberMapData.TypeConverterOptions.NumberStyle ?? NumberStyles.Integer;
                return short.TryParse(text, numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out short signedValue)
                    ? signedValue
                    : ushort.TryParse(text, numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out ushort legacyUnsignedValue)
                        ? (short)(legacyUnsignedValue - ushort.MaxValue - 1)
                        : base.ConvertFromString(text, row, memberMapData);
            }
        }
    }
}