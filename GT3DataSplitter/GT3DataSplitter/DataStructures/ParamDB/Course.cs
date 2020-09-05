using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Course : CsvDataStructure<CourseData, CourseCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
    public struct CourseData
    {
        public ulong Course;
        public ushort SurfaceType; // Normal / Wet / Dirt
        public ushort Filename;
        public ushort DisplayName;
        public ushort Unknown;
    }

    public sealed class CourseCSVMap : ClassMap<CourseData>
    {
        public CourseCSVMap()
        {
            Map(m => m.Course).TypeConverter(Utils.IdConverter);
            Map(m => m.SurfaceType);
            Map(m => m.Filename).TypeConverter(Program.Strings.Lookup);
            Map(m => m.DisplayName).TypeConverter(Program.UnicodeStrings.Lookup);
            Map(m => m.Unknown).TypeConverter(Program.Strings.Lookup);
        }
    }
}