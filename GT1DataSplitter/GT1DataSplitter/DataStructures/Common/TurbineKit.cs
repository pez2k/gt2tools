using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using TypeConverters;

    public class TurbineKit : CsvDataStructure<TurbineKitData, TurbineKitCSVMap>
    {
        public TurbineKit()
        {
            Header = "TURBINE";
            StringTableCount = 2;
            cacheFilename = true;
        }

        protected override string CreateOutputFilename()
        {
            string filename = base.CreateOutputFilename();
            return filename.Replace(Path.GetExtension(filename), $"_{Encoding.ASCII.GetString(data.Name[3..8])}_stage{data.Stage + 1:X2}{Path.GetExtension(filename)}");
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x38
    public struct TurbineKitData
    {
        public byte TorqueMultiplier1;
        public byte TorqueMultiplier2;
        public byte TorqueMultiplier3;
        public byte TorqueMultiplier4;
        public byte TorqueMultiplier5;
        public byte TorqueMultiplier6;
        public byte TorqueMultiplier7;
        public byte TorqueMultiplier8;
        public byte TorqueMultiplier9;
        public byte TorqueMultiplier10;
        public byte TorqueMultiplier11;
        public byte TorqueMultiplier12;
        public byte TorqueMultiplier13;
        public byte TorqueMultiplier14;
        public byte TorqueMultiplier15;
        public byte TorqueMultiplier16;
        public byte LevelDefault;
        public byte Unknown; // amount that each level scales the torque curve?
        public byte MaxPowerAtLevel; // lowest level at which power is maximum, setting level higher than this does nothing to peak power
        public byte Unknown2; // doesn't affect curve
        public byte Unknown3;
        public byte Unknown4;
        public byte Unknown5;
        public byte LevelMin;
        public byte LevelMax;
        public byte Padding;
        public ushort CarID;
        public ushort MufflerSoundUnusedMaybe; // possibly only set for racecars and some NA to turbo conversions?
        public ushort Stage;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public byte[] Name; // or are the last two chars actually bytes and no null terminator?
        public byte StageDuplicate;
        public uint Price;
        public ushort NamePart1;
        public ushort StringTablePart1;
        public ushort NamePart2;
        public ushort StringTablePart2;
    }

    public sealed class TurbineKitCSVMap : ClassMap<TurbineKitData>
    {
        public TurbineKitCSVMap(List<List<string>> tables)
        {
            Map(m => m.TorqueMultiplier1);
            Map(m => m.TorqueMultiplier2);
            Map(m => m.TorqueMultiplier3);
            Map(m => m.TorqueMultiplier4);
            Map(m => m.TorqueMultiplier5);
            Map(m => m.TorqueMultiplier6);
            Map(m => m.TorqueMultiplier7);
            Map(m => m.TorqueMultiplier8);
            Map(m => m.TorqueMultiplier9);
            Map(m => m.TorqueMultiplier10);
            Map(m => m.TorqueMultiplier11);
            Map(m => m.TorqueMultiplier12);
            Map(m => m.TorqueMultiplier13);
            Map(m => m.TorqueMultiplier14);
            Map(m => m.TorqueMultiplier15);
            Map(m => m.TorqueMultiplier16);
            Map(m => m.LevelDefault);
            Map(m => m.Unknown);
            Map(m => m.MaxPowerAtLevel);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Unknown5);
            Map(m => m.LevelMin);
            Map(m => m.LevelMax);
            Map(m => m.CarID).TypeConverter(new CachedCarIDConverter());
            Map(m => m.MufflerSoundUnusedMaybe);
            Map(m => m.Stage);
            Map(m => m.Name).TypeConverter(new HexStringConverter(11));
            Map(m => m.StageDuplicate);
            Map(m => m.Price);
            Map(m => m.NamePart1).TypeConverter(new StringTableLookup(tables[0]));
            Map(m => m.StringTablePart1).Convert(args => 0).Ignore();
            Map(m => m.NamePart2).TypeConverter(new StringTableLookup(tables[1]));
            Map(m => m.StringTablePart2).Convert(args => 1).Ignore();
        }
    }
}