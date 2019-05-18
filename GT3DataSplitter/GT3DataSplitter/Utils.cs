using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Collections.Generic;

namespace GT3.DataSplitter
{
    public static class Utils
    {
        public static DrivetrainTypeConverter DrivetrainTypeConverter { get; set; } = new DrivetrainTypeConverter();
        public static TyreStageConverter TyreStageConverter { get; set; } = new TyreStageConverter();
        public static TyreCompoundConverter TyreCompoundConverter { get; set; } = new TyreCompoundConverter();
        public static LicenseConverter LicenseConverter { get; set; } = new LicenseConverter();
        public static DrivetrainRestrictionConverter DrivetrainRestrictionConverter { get; set; } = new DrivetrainRestrictionConverter();

        public static CachedFileNameConverter GetFileNameConverter(string name)
        {
            return new CachedFileNameConverter(name);
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
            if (Name == "EligibleCars")
            {
                return (byte)stringNumber;
            }
            else if (Name == "Opponent")
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
