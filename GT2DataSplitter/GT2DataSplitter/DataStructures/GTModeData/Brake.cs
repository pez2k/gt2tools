﻿using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    public class Brake : CarCsvDataStructure<BrakeData, BrakeCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
    public struct BrakeData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte BrakingPower;
        public byte FrontBrakesUnknown;
        public byte RearBrakesUnknown;
    }

    public sealed class BrakeCSVMap : ClassMap<BrakeData>
    {
        public BrakeCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.BrakingPower);
            Map(m => m.FrontBrakesUnknown);
            Map(m => m.RearBrakesUnknown);
        }
    }
}
