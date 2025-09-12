using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Brake : MappedDataStructure<Brake.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte BrakingPower;
            public byte FrontBrakesUnknown;
            public byte RearBrakesUnknown;
        }

        public Models.Common.Brake MapToModel() =>
            new Models.Common.Brake
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                BrakingPower = data.BrakingPower,
                FrontBrakesUnknown = data.FrontBrakesUnknown,
                RearBrakesUnknown = data.RearBrakesUnknown
            };
    }
}