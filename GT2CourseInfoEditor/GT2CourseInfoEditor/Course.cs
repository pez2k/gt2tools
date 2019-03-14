using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.CourseInfoEditor
{
    using TrackNameConversion;

    public struct Course
    {
        public ushort DisplayName;
        public byte Unknown1;
        public byte Unknown2;
        public uint Filename;
        public bool IsNight;
        public bool IsEvening;
        public bool IsDirt;
        public bool Is2Player;
        public bool IsReverse;
        public bool IsPointToPoint;
        public bool Flag7;
        public byte Unknown4;
        public ushort Skybox;
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
    }

    public sealed class CourseCSVMap : ClassMap<Course>
    {
        public CourseCSVMap()
        {
            Map(m => m.DisplayName).TypeConverter<TrackDisplayNameConverter>();
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Filename).TypeConverter<TrackIdConverter>();
            Map(m => m.IsNight);
            Map(m => m.IsEvening);
            Map(m => m.IsDirt);
            Map(m => m.Is2Player);
            Map(m => m.IsReverse);
            Map(m => m.IsPointToPoint);
            Map(m => m.Flag7);
            Map(m => m.Unknown4); // ^
            Map(m => m.Skybox).TypeConverter<SkyboxNameConverter>();
            Map(m => m.Unknown5); // Car brightness ushort?
            Map(m => m.Unknown6); // ^
            Map(m => m.Unknown7); // ushort?
            Map(m => m.Unknown8); // ^
            Map(m => m.Unknown9); // ushort?
            Map(m => m.Unknown10); // ^
            Map(m => m.Unknown11); // uint pointer?
            Map(m => m.Unknown12);
            Map(m => m.Unknown13);
            Map(m => m.Unknown14);
            Map(m => m.Unknown15);
            Map(m => m.Unknown16);
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
            uint trackId = (uint)value;
            return trackId.ToTrackName();
        }
    }

    public class TrackDisplayNameConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return 0;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            ushort textAddress = (ushort)value;
            Program.DisplayNames.TryGetValue(textAddress, out string name);
            return name;
        }
    }

    public class SkyboxNameConverter : ITypeConverter
    {
        private readonly List<string> SkyboxNames = new List<string> { "au", "cartsky", "circle30sky", "circle80sky", "cloudtest", "dawn", "grinsky", "gv_sky", "indisky", "lagunasky", "licen_sky", "mskyX", "mskyX_2", "new_parmas_sky", "noon", "parma_sky", "romadark_sky", "romadsky", "roma_sh", "roma_sh_sky", "roma_sky", "sea_hare", "sea_ha_b", "sea_ha_c", "sea_ha_d", "sea_ha_e", "speedsky", "tesr_l2sky", "tl_sky2", "tl_sky2g", "tl_sky4", "tl_skyG", "tl_skyG2", "t_sky" };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return 0;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            ushort textAddress = (ushort)value;
            return SkyboxNames[textAddress];
        }
    }
}
