using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class LSD : MappedDataStructure<LSD.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x20
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte Unknown;
            public byte Unknown2;
            public byte Unknown3;
            public byte FrontUnknown; // diff type? 102 for null, 109 any upgrade & R34 rear, 118 for GTI, 110 for Cooper & R34 front, 104 for DC2
            public byte DefaultInitialFront;
            public byte MinInitialFront;
            public byte MaxInitialFront;
            public byte DefaultAccelFront;
            public byte MinAccelFront;
            public byte MaxAccelFront;
            public byte DefaultDecelFront;
            public byte MinDecelFront;
            public byte MaxDecelFront;
            public byte RearUnknown; // diff type?
            public byte DefaultInitialRear;
            public byte MinInitialRear;
            public byte MaxInitialRear;
            public byte DefaultAccelRear;
            public byte MinAccelRear;
            public byte MaxAccelRear;
            public byte DefaultDecelRear;
            public byte MinDecelRear;
            public byte MaxDecelRear;
        }

        public Models.Common.LSD MapToModel() =>
            new Models.Common.LSD
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                Unknown = data.Unknown,
                Unknown2 = data.Unknown2,
                Unknown3 = data.Unknown3,
                FrontUnknown = data.FrontUnknown,
                DefaultInitialFront = data.DefaultInitialFront,
                MinInitialFront = data.MinInitialFront,
                MaxInitialFront = data.MaxInitialFront,
                DefaultAccelFront = data.DefaultAccelFront,
                MinAccelFront = data.MinAccelFront,
                MaxAccelFront = data.MaxAccelFront,
                DefaultDecelFront = data.DefaultDecelFront,
                MinDecelFront = data.MinDecelFront,
                MaxDecelFront = data.MaxDecelFront,
                RearUnknown = data.RearUnknown,
                DefaultInitialRear = data.DefaultInitialRear,
                MinInitialRear = data.MinInitialRear,
                MaxInitialRear = data.MaxInitialRear,
                DefaultAccelRear = data.DefaultAccelRear,
                MinAccelRear = data.MinAccelRear,
                MaxAccelRear = data.MaxAccelRear,
                DefaultDecelRear = data.DefaultDecelRear,
                MinDecelRear = data.MinDecelRear,
                MaxDecelRear = data.MaxDecelRear
            };
    }
}