using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class Gear : CarCsvDataStructure<GearData, GearCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x24
    public struct GearData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte NumberOfGears;
        public ushort ReverseGearRatio;
        public ushort FirstGearRatio;
        public ushort SecondGearRatio;
        public ushort ThirdGearRatio;
        public ushort FourthGearRatio;
        public ushort FifthGearRatio;
        public ushort SixthGearRatio;
        public ushort SeventhGearRatio;
        public ushort DefaultFinalDriveRatio;
        public ushort MaxFinalDriveRatio;
        public ushort MinFinalDriveRatio;
        public byte AllowIndividualRatioAdjustments;
        public byte DefaultAutoSetting;
        public byte MinAutoSetting;
        public byte MaxAutoSetting;
    }

    public sealed class GearCSVMap : ClassMap<GearData>
    {
        public GearCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.NumberOfGears);
            Map(m => m.ReverseGearRatio);
            Map(m => m.FirstGearRatio);
            Map(m => m.SecondGearRatio);
            Map(m => m.ThirdGearRatio);
            Map(m => m.FourthGearRatio);
            Map(m => m.FifthGearRatio);
            Map(m => m.SixthGearRatio);
            Map(m => m.SeventhGearRatio);
            Map(m => m.DefaultFinalDriveRatio);
            Map(m => m.MaxFinalDriveRatio);
            Map(m => m.MinFinalDriveRatio);
            Map(m => m.AllowIndividualRatioAdjustments);
            Map(m => m.DefaultAutoSetting);
            Map(m => m.MinAutoSetting);
            Map(m => m.MaxAutoSetting);
        }
    }
}
