using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class NATune : MappedDataStructure<NATune.Data, Models.Common.NATune>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x0C
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public sbyte PowerbandRPMIncrease;
            public sbyte RPMIncrease;
            public byte PowerMultiplier;
        }

        public override Models.Common.NATune MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.NATune
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                PowerbandRPMIncrease = data.PowerbandRPMIncrease,
                RPMIncrease = data.RPMIncrease,
                PowerMultiplier = data.PowerMultiplier
            };
    }
}