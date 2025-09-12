using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class RacingModify : MappedDataStructure<RacingModify.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x1C
        public struct Data
        {
            public uint CarId;
            public uint Price; // if 0 or low byte = 0, not possible
            public uint BodyId;
            public byte Weight; // weight is a multiple of some car-indepenent value
            public byte BodyRollAmount;
            public byte Stage;
            public byte Drag;
            public byte FrontDownforceMinimum;
            public byte FrontDownforceMaximum;
            public byte FrontDownforceDefault;
            public byte RearDownforceMinimum;
            public byte RearDownforceMaximum;
            public byte RearDownforceDefault;
            public byte Unknown3;
            public byte Unknown4;
            public byte Unknown5;
            public byte Unknown6;
            public ushort Width; // rm car width - in mm
        }

        public Models.Common.RacingModify MapToModel() =>
            new Models.Common.RacingModify
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                BodyId = data.BodyId,
                Weight = data.Weight,
                BodyRollAmount = data.BodyRollAmount,
                Stage = data.Stage,
                Drag = data.Drag,
                FrontDownforceMinimum = data.FrontDownforceMinimum,
                FrontDownforceMaximum = data.FrontDownforceMaximum,
                FrontDownforceDefault = data.FrontDownforceDefault,
                RearDownforceMinimum = data.RearDownforceMinimum,
                RearDownforceMaximum = data.RearDownforceMaximum,
                RearDownforceDefault = data.RearDownforceDefault,
                Unknown3 = data.Unknown3,
                Unknown4 = data.Unknown4,
                Unknown5 = data.Unknown5,
                Unknown6 = data.Unknown6,
                Width = data.Width
            };
    }
}