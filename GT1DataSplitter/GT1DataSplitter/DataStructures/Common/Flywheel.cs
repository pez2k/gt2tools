using System.Collections.Generic;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class Flywheel : CsvDataStructure<FlywheelData, FlywheelCSVMap>
    {
        public Flywheel()
        {
            Header = "FLYWHEL";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename() => CreateDetailedOutputFilename(0x4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
    public struct FlywheelData
    {
        public byte FlywheelInertiaMultiplier;
        public byte EngineBrakingMultiplier;
        public byte FrontWheelInertiaMultiplier;
        public byte RearWheelInertiaMultiplier;
        public ushort CarID;
        public byte Stage;
        public byte StageDuplicate;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class FlywheelCSVMap : ClassMap<FlywheelData>
    {
        public FlywheelCSVMap(List<List<string>> tables)
        {
            Map(m => m.FlywheelInertiaMultiplier);
            Map(m => m.EngineBrakingMultiplier);
            Map(m => m.FrontWheelInertiaMultiplier);
            Map(m => m.RearWheelInertiaMultiplier);
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