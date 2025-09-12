using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Flywheel : MappedDataStructure<Flywheel.Data>
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

        public Models.Common.Flywheel MapToModel() =>
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