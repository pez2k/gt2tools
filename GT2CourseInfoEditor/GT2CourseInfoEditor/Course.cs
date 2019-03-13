using System.Runtime.InteropServices;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.CourseInfoEditor
{
    using TrackNameConversion;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Course
    {
        public byte Unknown1;
        public byte Unknown2;
        public byte Unknown3;
        public byte Unknown4;
        public uint Filename;
        public byte Unknown5;
        public byte Unknown6;
        public byte Unknown7;
        public byte Unknown8;
        public byte Unknown9;
        public byte Unknown10;
        public byte Unknown11;
        public byte Unknown12;
        public byte Unknown13;
        public byte Unknown14;
        public byte Unknown15;
        public byte Unknown16;
        public byte Unknown17;
        public byte Unknown18;
        public byte Unknown19;
    }

    public sealed class CourseCSVMap : ClassMap<Course>
    {
        public CourseCSVMap()
        {
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Filename).TypeConverter<TrackIdConverter>();
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.Unknown10);
            Map(m => m.Unknown11);
            Map(m => m.Unknown12);
            Map(m => m.Unknown13);
            Map(m => m.Unknown14);
            Map(m => m.Unknown15);
            Map(m => m.Unknown16);
            Map(m => m.Unknown17);
            Map(m => m.Unknown18);
            Map(m => m.Unknown19);
        }
    }

    public class TrackIdConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text.ToTrackID();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            uint carId = (uint)value;
            return carId.ToTrackName();
        }
    }
}
