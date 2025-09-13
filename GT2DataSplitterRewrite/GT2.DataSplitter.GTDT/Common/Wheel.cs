using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    public class Wheel : MappedDataStructure<Wheel.Data, Models.Common.Wheel>
    {
        private readonly string[] wheelManufacturers =
        [
            "bb",
            "br",
            "du",
            "en",
            "fa",
            "oz",
            "ra",
            "sp",
            "yo"
        ];

        private readonly string[] wheelLugs = [ "-", "4", "5", "6" ];

        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x08, maybe
        public struct Data
        {
            public uint WheelId;
            public byte StageMaybe; // always 0 for unnamed, 1 for named
            public byte Unknown; // always 0
            public byte Unknown2; // always 1 for named, 0-3 for unnamed
            public byte Unknown3; // always 2 for named, 0-3 for unnamed
        }

        public override Models.Common.Wheel MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.Wheel
            {
                WheelId = ToWheelName(data.WheelId),
                StageMaybe = data.StageMaybe,
                Unknown = data.Unknown,
                Unknown2 = data.Unknown2,
                Unknown3 = data.Unknown3
            };

        private string ToWheelName(uint value)
        {
            if (value == 0)
            {
                return "";
            }

            string manufacturer = wheelManufacturers[(value >> 24) / 0x10];
            uint wheelNumber = (value >> 16) & 0xFF;
            string lugs = wheelLugs[((value >> 8) & 0xFF) / 0x20];
            char colour = (char)(value & 0xFF);
            return $"{manufacturer}{wheelNumber:D3}-{lugs}{colour}";
        }
    }
}