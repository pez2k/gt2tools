using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Chassis : MappedDataStructure<Chassis.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
        public struct Data
        {
            public uint CarId;
            public byte FrontWeightDistribution;
            public byte Unknown2;
            public byte FrontGrip;
            public byte RearGrip;
            public ushort Length;
            public ushort Height;
            public ushort Wheelbase;
            public ushort Weight;
            public byte TurningResistance;
            public byte PitchResistance;
            public byte RollResistance;
            public byte Unknown8;
        }

        public Models.Common.Chassis MapToModel() =>
            new Models.Common.Chassis
            {
                CarId = data.CarId.ToCarName(),
                FrontWeightDistribution = data.FrontWeightDistribution,
                Unknown2 = data.Unknown2,
                FrontGrip = data.FrontGrip,
                RearGrip = data.RearGrip,
                Length = data.Length,
                Height = data.Height,
                Wheelbase = data.Wheelbase,
                Weight = data.Weight,
                TurningResistance = data.TurningResistance,
                PitchResistance = data.PitchResistance,
                RollResistance = data.RollResistance,
                Unknown8 = data.Unknown8
            };
    }
}