using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Flywheel : MappedDataStructure<Flywheel.Data, Models.Common.Flywheel>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte RPMDropRate;
            public byte ShiftDelay;
            public byte InertialWeight;
        }

        public override Models.Common.Flywheel MapToModel(UnicodeStringTable unicode, ASCIIStringTable ascii) =>
            new Models.Common.Flywheel
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                RPMDropRate = data.RPMDropRate,
                ShiftDelay = data.ShiftDelay,
                InertialWeight = data.InertialWeight
            };
    }
}