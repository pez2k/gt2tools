using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class TiresFront : MappedDataStructure<TiresFront.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte SteeringReaction1;
            public byte WheelSize;
            public byte SteeringReaction2;
            public byte TireCompound;
            public byte TireForceVolMaybe;
            public byte SlipMultiplier;
            public byte GripMultiplier;
        }

        public Models.Common.TiresFront MapToModel() =>
            new Models.Common.TiresFront
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                SteeringReaction1 = data.SteeringReaction1,
                WheelSize = data.WheelSize,
                SteeringReaction2 = data.SteeringReaction2,
                TireCompound = data.TireCompound,
                TireForceVolMaybe = data.TireForceVolMaybe,
                SlipMultiplier = data.SlipMultiplier,
                GripMultiplier = data.GripMultiplier
            };
    }
}