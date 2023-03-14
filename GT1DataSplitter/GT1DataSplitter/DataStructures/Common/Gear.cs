using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class Gear : CsvDataStructure<GearData, GearCSVMap>
    {
        public Gear()
        {
            Header = "GEAR";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x32);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x44
    public struct GearData
    {
        public byte NumberOfGears;
        public byte StageDuplicate;
        public ushort FirstGearRatioDefault;
        public ushort SecondGearRatioDefault;
        public ushort ThirdGearRatioDefault;
        public ushort FourthGearRatioDefault;
        public ushort FifthGearRatioDefault;
        public ushort SixthGearRatioDefault;
        public ushort SeventhGearRatioDefault;
        public ushort FinalDriveRatioDefault;
        public ushort FirstGearRatioMin;
        public ushort FirstGearRatioMax;
        public ushort SecondGearRatioMin;
        public ushort SecondGearRatioMax;
        public ushort ThirdGearRatioMin;
        public ushort ThirdGearRatioMax;
        public ushort FourthGearRatioMin;
        public ushort FourthGearRatioMax;
        public ushort FifthGearRatioMin;
        public ushort FifthGearRatioMax;
        public ushort SixthGearRatioMin;
        public ushort SixthGearRatioMax;
        public ushort SeventhGearRatioMin;
        public ushort SeventhGearRatioMax;
        public ushort FinalDriveRatioMin;
        public ushort FinalDriveRatioMax;
        public ushort CarID;
        public ushort Stage;
        public ushort Padding;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class GearCSVMap : ClassMap<GearData>
    {
        public GearCSVMap(List<List<string>> tables)
        {
            Map(m => m.NumberOfGears);
            Map(m => m.StageDuplicate);
            Map(m => m.FirstGearRatioDefault);
            Map(m => m.SecondGearRatioDefault);
            Map(m => m.ThirdGearRatioDefault);
            Map(m => m.FourthGearRatioDefault);
            Map(m => m.FifthGearRatioDefault);
            Map(m => m.SixthGearRatioDefault);
            Map(m => m.SeventhGearRatioDefault);
            Map(m => m.FinalDriveRatioDefault);
            Map(m => m.FirstGearRatioMin);
            Map(m => m.FirstGearRatioMax);
            Map(m => m.SecondGearRatioMin);
            Map(m => m.SecondGearRatioMax);
            Map(m => m.ThirdGearRatioMin);
            Map(m => m.ThirdGearRatioMax);
            Map(m => m.FourthGearRatioMin);
            Map(m => m.FourthGearRatioMax);
            Map(m => m.FifthGearRatioMin);
            Map(m => m.FifthGearRatioMax);
            Map(m => m.SixthGearRatioMin);
            Map(m => m.SixthGearRatioMax);
            Map(m => m.SeventhGearRatioMin);
            Map(m => m.SeventhGearRatioMax);
            Map(m => m.FinalDriveRatioMin);
            Map(m => m.FinalDriveRatioMax);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Stage);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}