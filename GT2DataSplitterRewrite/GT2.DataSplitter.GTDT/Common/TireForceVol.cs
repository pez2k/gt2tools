using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    public class TireForceVol : MappedDataStructure<TireForceVol.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x08
        public struct Data
        {
            public byte TarmacGrip;
            public byte GuideGrip; // kerbs
            public byte GreenGrip; // grass
            public byte SandGrip;
            public byte GravelGrip;
            public byte DirtGrip;
            public byte WaterGripMaybe; // wet, unused?
            public byte Padding;
        }

        public Models.Common.TireForceVol MapToModel() =>
            new Models.Common.TireForceVol
            {
                TarmacGrip = data.TarmacGrip,
                GuideGrip = data.GuideGrip,
                GreenGrip = data.GreenGrip,
                SandGrip = data.SandGrip,
                GravelGrip = data.GravelGrip,
                DirtGrip = data.DirtGrip,
                WaterGripMaybe = data.WaterGripMaybe
            };
    }
}