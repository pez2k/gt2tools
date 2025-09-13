using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class PropellerShaft : MappedDataStructure<PropellerShaft.Data, Models.Common.PropellerShaft>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte RPMDropRate;
            public byte Inertia;
            public byte Inertia2;
        }

        public override Models.Common.PropellerShaft MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.PropellerShaft
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                RPMDropRate = data.RPMDropRate,
                Inertia = data.Inertia,
                Inertia2 = data.Inertia2
            };
    }
}