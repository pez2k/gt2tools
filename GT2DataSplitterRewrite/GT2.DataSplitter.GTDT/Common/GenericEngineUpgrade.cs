using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class GenericEngineUpgrade : MappedDataStructure<GenericEngineUpgrade.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public sbyte PowerbandScaling;
            public sbyte RPMIncrease;
            public byte PowerMultiplier;
        }

        public TModel MapToModel<TModel>() where TModel : Models.Common.GenericEngineUpgrade, new() =>
            new TModel
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                PowerbandScaling = data.PowerbandScaling,
                RPMIncrease = data.RPMIncrease,
                PowerMultiplier = data.PowerMultiplier
            };
    }
}