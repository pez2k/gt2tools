using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.CourseInfoEditor
{
    using TrackNameConversion;

    public struct Course
    {
        public string DisplayName;
        public uint Filename;
        public bool IsNight;
        public bool IsEvening;
        public bool IsDirt;
        public bool Is2Player;
        public bool IsReverse;
        public bool IsPointToPoint;
        public bool Flag7;
        public byte Padding;
        public ushort Skybox;
        public string LightingArea1Colour;
        public ushort LightingArea1ColourMultiplier;
        public string LightingArea2Colour;
        public ushort LightingArea2ColourMultiplier;
        public string LightingArea3Colour;
        public ushort LightingArea3ColourMultiplier;
    }

    public sealed class CourseCSVMap : ClassMap<Course>
    {
        public CourseCSVMap()
        {
            Map(m => m.DisplayName);
            Map(m => m.Filename).TypeConverter<TrackIdConverter>();
            Map(m => m.IsNight);
            Map(m => m.IsEvening);
            Map(m => m.IsDirt);
            Map(m => m.Is2Player);
            Map(m => m.IsReverse);
            Map(m => m.IsPointToPoint);
            Map(m => m.Flag7);
            Map(m => m.Skybox).TypeConverter<SkyboxNameConverter>();
            Map(m => m.LightingArea1Colour);
            Map(m => m.LightingArea1ColourMultiplier);
            Map(m => m.LightingArea2Colour);
            Map(m => m.LightingArea2ColourMultiplier);
            Map(m => m.LightingArea3Colour);
            Map(m => m.LightingArea3ColourMultiplier);
        }
    }

    public class TrackIdConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => text.ToTrackID();

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            uint trackId = (uint)value;
            return trackId.ToTrackName();
        }
    }

    public class SkyboxNameConverter : ITypeConverter
    {
        private readonly string[] skyboxNames = {
            "au", "cartsky", "circle30sky", "circle80sky", "cloudtest", "dawn", "grinsky", "gv_sky", "indisky", "lagunasky", "licen_sky", "mskyX", "mskyX_2", "new_parmas_sky",
            "noon", "parma_sky", "roma_sh", "roma_sh_sky", "roma_sky", "romadark_sky", "romadsky", "sea_ha_b", "sea_ha_c", "sea_ha_d", "sea_ha_e", "sea_hare", "speedsky", "t_sky",
            "tesr_l2sky", "tl_sky2", "tl_sky2g", "tl_sky4", "tl_skyG", "tl_skyG2"
        };

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            for (int i = 0; i < skyboxNames.Length; i++)
            {
                if (skyboxNames[i] == text)
                {
                    return (ushort)i;
                }
            }
            return ushort.TryParse(text, out ushort numericValue) ? numericValue
                                                                  : throw new Exception($"Unrecognised skybox name: {text}");
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            ushort skyboxNumber = (ushort)value;
            return skyboxNumber < skyboxNames.Length ? skyboxNames[skyboxNumber]
                                                     : $"{skyboxNumber}";
        }
    }
}