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
        public static TireStageConverter TireStageConverter { get; set; } = new TireStageConverter();
        public static TireCompoundConverter TireCompoundConverter { get; set; } = new TireCompoundConverter();
        public static LicenseConverter LicenseConverter { get; set; } = new LicenseConverter();
        public static DrivetrainRestrictionConverter DrivetrainRestrictionConverter { get; set; } = new DrivetrainRestrictionConverter();

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
            int stringNumber = FileNameCache.Get(Name, text);
            if (Name == nameof(Regulations))
            {
                return (byte)stringNumber;
            }
            else if (Name == nameof(EnemyCars))
            {
                return (uint)stringNumber;
            }
            else
            {
                return (ushort)stringNumber;
            }
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            int cacheIndex = int.Parse(value.ToString());
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

    public class TireStageConverter : ITypeConverter
    {
        protected List<string> TireTypes = new List<string> { "Stock", "Sports", "Hard", "Medium", "Soft", "SuperSoft", "Simulation", "Dirt" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)TireTypes.IndexOf(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte tyreType = (byte)value;
            return TireTypes[tyreType];
        }
    }

    public class TireCompoundConverter : ITypeConverter
    {
        protected List<string> TireCompounds = new List<string> { "RoadFront", "RoadRear", "SportsFront", "SportsRear", "HardFront", "HardRear", "MediumFront", "MediumRear", "SoftFront", "SoftRear", "SuperSoftFront", "SuperSoftRear", "DirtFront", "DirtRear", "RWDDirtFront", "RWDDirtRear", "SimulationFront", "SimulationRear", "PikesPeakFront", "PikesPeakRear" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)TireCompounds.IndexOf(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte tyreCompound = (byte)value;
            return TireCompounds[tyreCompound];
        }
    }

    public class LicenseConverter : ITypeConverter
    {
        protected List<string> LicenseTypes = new List<string> { "None", "B", "A", "IC", "IB", "IA", "S" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (byte)LicenseTypes.IndexOf(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            byte licenseType = (byte)value;
            return LicenseTypes[licenseType];
        }
    }

    public class DrivetrainRestrictionConverter : ITypeConverter
    {
        protected List<string> DrivetrainTypes = new List<string> { "None", "FF", "FR", "MR", "RR", "4WD" };

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
}
