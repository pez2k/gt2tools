using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class GearConcept : CsvDataStructure<GearConceptData, GearConceptCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x30
    public struct GearConceptData
    {
        public ulong Part;
        public ulong Car;
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
        public uint Price;
        public uint ModeMaybe;
        public uint Unknown;
    }

    public sealed class GearConceptCSVMap : ClassMap<GearConceptData>
    {
        public GearConceptCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
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
            Map(m => m.Price);
            Map(m => m.ModeMaybe);
            Map(m => m.Unknown);
        }
    }
}