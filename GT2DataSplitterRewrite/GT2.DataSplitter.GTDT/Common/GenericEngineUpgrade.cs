using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
    public struct GenericEngineUpgradeData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public sbyte PowerbandScaling;
        public sbyte RPMIncrease;
        public byte PowerMultiplier;
    }

    public class GenericEngineUpgrade<TModel> : MappedDataStructure<GenericEngineUpgradeData, TModel> where TModel : Models.Common.GenericEngineUpgrade, new()
    {
        public override TModel MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
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