using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Lightweight : MappedDataStructure<Lightweight.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0xC
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public ushort Weight;
            public byte Unknown;
            public byte Stage;
        }

        public Models.Common.Lightweight MapToModel() =>
            new Models.Common.Lightweight
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Weight = data.Weight,
                Unknown = data.Unknown,
                Stage = data.Stage
            };
    }
}