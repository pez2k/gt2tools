using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class BrakeController : MappedDataStructure<BrakeController.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte MaxFrontBias;
            public byte Unknown;
            public byte Unknown2;
            public byte DefaultBias;
            public byte MaxRearBias;
            public byte Unknown3;
            public byte Unknown4;
        }

        public Models.Common.BrakeController MapToModel() =>
            new Models.Common.BrakeController
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                MaxFrontBias = data.MaxFrontBias,
                Unknown = data.Unknown,
                Unknown2 = data.Unknown2,
                DefaultBias = data.DefaultBias,
                MaxRearBias = data.MaxRearBias,
                Unknown3 = data.Unknown3,
                Unknown4 = data.Unknown4
            };
    }
}