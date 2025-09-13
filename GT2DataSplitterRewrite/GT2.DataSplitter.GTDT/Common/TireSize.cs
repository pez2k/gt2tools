using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    public class TireSize : MappedDataStructure<TireSize.Data, Models.Common.TireSize>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x04
        public struct Data
        {
            public byte DiameterInches;
            public byte WidthMM;
            public byte Profile;
            public byte Padding;
        }

        public override Models.Common.TireSize MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.TireSize
            {
                DiameterInches = data.DiameterInches,
                WidthMM = data.WidthMM,
                Profile = data.Profile,
                Padding = data.Padding
            };
    }
}