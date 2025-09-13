using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Clutch : MappedDataStructure<Clutch.Data, Models.Common.Clutch>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte RPMDropRate;
            public byte InertiaDisengaged;
            public byte InertiaEngaged;
            public byte InertialWeight;
            public byte InertiaBraking;
            public byte Unknown1;
            public byte Unknown2;
        }

        public override Models.Common.Clutch MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.Clutch
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                RPMDropRate = data.RPMDropRate,
                InertiaDisengaged = data.InertiaDisengaged,
                InertiaEngaged = data.InertiaEngaged,
                InertialWeight = data.InertialWeight,
                InertiaBraking = data.InertiaBraking,
                Unknown1 = data.Unknown1,
                Unknown2 = data.Unknown2
            };
    }
}