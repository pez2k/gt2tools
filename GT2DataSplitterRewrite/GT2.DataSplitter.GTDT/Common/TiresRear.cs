using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class TiresRear : MappedDataStructure<TiresRear.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x0C
        public struct Data
        {
            public uint CarId;
            public byte Stage;
            public byte SteeringReaction1;
            public byte WheelSize;
            public byte SteeringReaction2;
            public byte TireCompound;
            public byte TireForceVolMaybe;
            public byte SlipMultiplier;
            public byte GripMultiplier;
        }

        public Models.Common.TiresRear MapToModel() =>
            new Models.Common.TiresRear
            {
                CarId = data.CarId.ToCarName(),
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