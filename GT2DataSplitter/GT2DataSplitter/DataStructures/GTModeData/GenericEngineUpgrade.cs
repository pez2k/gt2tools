﻿using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class GenericEngineUpgrade : CarCsvDataStructure<GenericEngineUpgradeData, GenericEngineUpgradeCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
    public struct GenericEngineUpgradeData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public sbyte PowerbandScaling;
        public sbyte RPMIncrease;
        public byte PowerMultiplier;
    }

    public sealed class GenericEngineUpgradeCSVMap : ClassMap<GenericEngineUpgradeData>
    {
        public GenericEngineUpgradeCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.PowerbandScaling);
            Map(m => m.RPMIncrease);
            Map(m => m.PowerMultiplier);
        }
    }
}
