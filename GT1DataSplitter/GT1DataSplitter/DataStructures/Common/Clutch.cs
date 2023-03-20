using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class Clutch : CsvDataStructure<ClutchData, ClutchCSVMap>
    {
        public Clutch()
        {
            Header = "CLUTCH";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x6);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x18
    public struct ClutchData
    {
        public byte EngineBrakingMultiplier;
        public byte FlywheelInertiaMultiplier;
        public byte FrontWheelInertiaMultiplier;
        public byte RearWheelInertiaMultiplier;
        public byte Unknown;
        public byte ClutchTorqueKGFM;
        public ushort CarID;
        public byte Stage;
        public byte StageDuplicate;
        public ushort Padding;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class ClutchCSVMap : ClassMap<ClutchData>
    {
        public ClutchCSVMap(List<List<string>> tables)
        {
            Map(m => m.EngineBrakingMultiplier);
            Map(m => m.FlywheelInertiaMultiplier);
            Map(m => m.FrontWheelInertiaMultiplier);
            Map(m => m.RearWheelInertiaMultiplier);
            Map(m => m.Unknown);
            Map(m => m.ClutchTorqueKGFM);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.Stage);
            Map(m => m.StageDuplicate);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}