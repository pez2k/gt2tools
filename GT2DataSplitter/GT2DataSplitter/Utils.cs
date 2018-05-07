using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public static class Utils
    {
        public static CarIdConverter CarIdConverter { get; set; } = new CarIdConverter();
        public static CarIdArrayConverter CarIdArrayConverter { get; set; } = new CarIdArrayConverter();
        public static DrivetrainTypeConverter DrivetrainTypeConverter { get; set; } = new DrivetrainTypeConverter();
        public static TyreStageConverter TyreStageConverter { get; set; } = new TyreStageConverter();
        public static TyreCompoundConverter TyreCompoundConverter { get; set; } = new TyreCompoundConverter();

        public static CachedFileNameConverter GetFileNameConverter(string name)
        {
            return new CachedFileNameConverter(name);
        }
    }

    public class CarIdConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text.ToCarID();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            uint carId = (uint)value;
            return carId.ToCarName();
        }
    }

    public class CarIdArrayConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string[] inputs = text.Split(',');
            uint[] carIds = new uint[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                carIds[i] = inputs[i].ToCarID();
            }

            return carIds;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            uint[] carIds = ((Array)value).Cast<uint>().ToArray();
            string output = "";
            foreach (uint carId in carIds)
            {
                output += carId.ToCarName() + ",";
            }
            return output.TrimEnd(',');
        }
    }

    public class CachedFileNameConverter : ITypeConverter
    {
        public virtual string Name { get; set; }

        public CachedFileNameConverter(string name = "")
        {
            Name = name;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return FileNameCache.Get(Name, text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            ushort cacheIndex = (ushort)value;
            return FileNameCache.Get(Name, cacheIndex);
        }
    }

    public class DrivetrainTypeConverter : ITypeConverter
    {
        protected List<string> DrivetrainTypes = new List<string> { "FR", "FF", "4WD", "MR", "RR" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)DrivetrainTypes.IndexOf(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte drivetrainType = (byte)value;
            return DrivetrainTypes[drivetrainType];
        }
    }

    public class TyreStageConverter : ITypeConverter
    {
        protected List<string> TyreTypes = new List<string> { "Stock", "Sports", "Hard", "Medium", "Soft", "SuperSoft", "Simulation", "Dirt" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)TyreTypes.IndexOf(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte tyreType = (byte)value;
            return TyreTypes[tyreType];
        }
    }

    public class TyreCompoundConverter : ITypeConverter
    {
        protected List<string> TyreTypes = new List<string> { "RoadFront", "RoadRear", "SportsFront", "SportsRear", "HardFront", "HardRear", "MediumFront", "MediumRear", "SoftFront", "SoftRear", "SuperSoftFront", "SuperSoftRear", "DirtFront", "DirtRear", "RWDDirtFront", "RWDDirtRear", "SimulationFront", "SimulationRear", "PikesPeakFront", "PikesPeakRear" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)(TyreTypes.IndexOf(text));
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte tyreType = (byte)value;
            return TyreTypes[tyreType];
        }
    }
}
