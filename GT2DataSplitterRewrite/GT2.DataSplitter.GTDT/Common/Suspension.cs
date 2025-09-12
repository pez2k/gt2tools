using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Suspension : MappedDataStructure<Suspension.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x4C
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte MinCamberFront;
            public byte MaxCamberFront;
            public byte DefaultCamberFront;
            public byte MinCamberRear;
            public byte MaxCamberRear;
            public byte DefaultCamberRear;
            public byte MinToeFront;
            public byte MaxToeFront; // no default?
            public byte MinToeRear;
            public byte MaxToeRear;
            public byte MinHeightFront;
            public byte MaxHeightFront;
            public byte DefaultHeightFront;
            public byte MinHeightRear;
            public byte MaxHeightRear;
            public byte DefaultHeightRear;
            public byte DampingFront;
            public byte DampingRear;
            public byte TravelFront;
            public byte TravelRear;
            public byte MinSpringRateFront;
            public byte MaxSpringRateFront;
            public byte DefaultSpringRateFront;
            public byte MinSpringRateRear;
            public byte MaxSpringRateRear;
            public byte DefaultSpringRateRear;
            public byte SpringFrequencyFront;
            public byte SpringFrequencyRear;
            public byte Unknown7;
            public byte Unknown8;
            public byte MaxDamperBoundFront;
            public byte Unknown9;
            public byte Unknown10;
            public byte DefaultDamperBoundFront;
            public byte Unknown11;
            public byte Unknown12;
            public byte Unknown13;
            public byte MaxDamperReboundFront;
            public byte Unknown14;
            public byte Unknown15;
            public byte DefaultDamperReboundFront;
            public byte Unknown16;
            public byte Unknown17;
            public byte Unknown18;
            public byte MaxDamperBoundRear;
            public byte Unknown19;
            public byte Unknown20;
            public byte DefaultDamperBoundRear;
            public byte Unknown21;
            public byte Unknown22;
            public byte Unknown23;
            public byte MaxDamperReboundRear;
            public byte Unknown24;
            public byte Unknown25;
            public byte DefaultDamperReboundRear;
            public byte Unknown26;
            public byte Unknown27;
            public byte Unknown28;
            public byte MaxStabiliserFront; // steps
            public byte Unknown29; // probably min value
            public byte Unknown30; // probably max value
            public byte DefaultStabiliserFront;
            public byte MaxStabiliserRear; // steps
            public byte Unknown31; // probably min value
            public byte Unknown32; // probably max value
            public byte DefaultStabiliserRear;
            public byte Unknown33;
        }

        public Models.Common.Suspension MapToModel() =>
            new Models.Common.Suspension
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                MinCamberFront = data.MinCamberFront,
                MaxCamberFront = data.MaxCamberFront,
                DefaultCamberFront = data.DefaultCamberFront,
                MinCamberRear = data.MinCamberRear,
                MaxCamberRear = data.MaxCamberRear,
                DefaultCamberRear = data.DefaultCamberRear,
                MinToeFront = data.MinToeFront,
                MaxToeFront = data.MaxToeFront,
                MinToeRear = data.MinToeRear,
                MaxToeRear = data.MaxToeRear,
                MinHeightFront = data.MinHeightFront,
                MaxHeightFront = data.MaxHeightFront,
                DefaultHeightFront = data.DefaultHeightFront,
                MinHeightRear = data.MinHeightRear,
                MaxHeightRear = data.MaxHeightRear,
                DefaultHeightRear = data.DefaultHeightRear,
                DampingFront = data.DampingFront,
                DampingRear = data.DampingRear,
                TravelFront = data.TravelFront,
                TravelRear = data.TravelRear,
                MinSpringRateFront = data.MinSpringRateFront,
                MaxSpringRateFront = data.MaxSpringRateFront,
                DefaultSpringRateFront = data.DefaultSpringRateFront,
                MinSpringRateRear = data.MinSpringRateRear,
                MaxSpringRateRear = data.MaxSpringRateRear,
                DefaultSpringRateRear = data.DefaultSpringRateRear,
                SpringFrequencyFront = data.SpringFrequencyFront,
                SpringFrequencyRear = data.SpringFrequencyRear,
                Unknown7 = data.Unknown7,
                Unknown8 = data.Unknown8,
                MaxDamperBoundFront = data.MaxDamperBoundFront,
                Unknown9 = data.Unknown9,
                Unknown10 = data.Unknown10,
                DefaultDamperBoundFront = data.DefaultDamperBoundFront,
                Unknown11 = data.Unknown11,
                Unknown12 = data.Unknown12,
                Unknown13 = data.Unknown13,
                MaxDamperReboundFront = data.MaxDamperReboundFront,
                Unknown14 = data.Unknown14,
                Unknown15 = data.Unknown15,
                DefaultDamperReboundFront = data.DefaultDamperReboundFront,
                Unknown16 = data.Unknown16,
                Unknown17 = data.Unknown17,
                Unknown18 = data.Unknown18,
                MaxDamperBoundRear = data.MaxDamperBoundRear,
                Unknown19 = data.Unknown19,
                Unknown20 = data.Unknown20,
                DefaultDamperBoundRear = data.DefaultDamperBoundRear,
                Unknown21 = data.Unknown21,
                Unknown22 = data.Unknown22,
                Unknown23 = data.Unknown23,
                MaxDamperReboundRear = data.MaxDamperReboundRear,
                Unknown24 = data.Unknown24,
                Unknown25 = data.Unknown25,
                DefaultDamperReboundRear = data.DefaultDamperReboundRear,
                Unknown26 = data.Unknown26,
                Unknown27 = data.Unknown27,
                Unknown28 = data.Unknown28,
                MaxStabiliserFront = data.MaxStabiliserFront,
                Unknown29 = data.Unknown29,
                Unknown30 = data.Unknown30,
                DefaultStabiliserFront = data.DefaultStabiliserFront,
                MaxStabiliserRear = data.MaxStabiliserRear,
                Unknown31 = data.Unknown31,
                Unknown32 = data.Unknown32,
                DefaultStabiliserRear = data.DefaultStabiliserRear,
                Unknown33 = data.Unknown33
            };
    }
}