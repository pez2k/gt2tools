using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class TurbineKit : MappedDataStructure<TurbineKit.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x14
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte BoostGaugeLimit;
            public byte LowRPMBoost;
            public byte HighRPMBoost;
            public byte SpoolRate;
            public byte Unknown1;
            public byte Unknown2;
            public byte Unknown3;
            public sbyte RPMIncrease;
            public sbyte RedlineIncrease;
            public byte HighRPMPowerMultiplier;
            public byte LowRPMPowerMultiplier;
        }

        public Models.Common.TurbineKit MapToModel() =>
            new Models.Common.TurbineKit
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                BoostGaugeLimit = data.BoostGaugeLimit,
                LowRPMBoost = data.LowRPMBoost,
                HighRPMBoost = data.HighRPMBoost,
                SpoolRate = data.SpoolRate,
                Unknown1 = data.Unknown1,
                Unknown2 = data.Unknown2,
                Unknown3 = data.Unknown3,
                RPMIncrease = data.RPMIncrease,
                RedlineIncrease = data.RedlineIncrease,
                HighRPMPowerMultiplier = data.HighRPMPowerMultiplier,
                LowRPMPowerMultiplier = data.LowRPMPowerMultiplier
            };
    }
}