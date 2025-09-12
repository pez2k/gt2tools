using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Steer : MappedDataStructure<Steer.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x18
        public struct Data
        {
            public uint CarId;
            public uint Price;
            public byte Stage;
            public byte Unknown1;
            public byte Angle1Speed;
            public byte Angle2Speed;
            public byte Angle3Speed;
            public byte Angle4Speed;
            public byte Angle5Speed;
            public byte Angle6Speed;
            public byte Angle1;
            public byte Angle2;
            public byte Angle3;
            public byte Angle4;
            public byte Angle5;
            public byte Angle6;
            public byte MaxSteeringAngle;
            public byte Unknown2;
        }

        public Models.Common.Steer MapToModel() =>
            new Models.Common.Steer
            {
                CarId = data.CarId.ToCarName(),
                Price = data.Price,
                Stage = data.Stage,
                Unknown1 = data.Unknown1,
                Angle1Speed = data.Angle1Speed,
                Angle2Speed = data.Angle2Speed,
                Angle3Speed = data.Angle3Speed,
                Angle4Speed = data.Angle4Speed,
                Angle5Speed = data.Angle5Speed,
                Angle6Speed = data.Angle6Speed,
                Angle1 = data.Angle1,
                Angle2 = data.Angle2,
                Angle3 = data.Angle3,
                Angle4 = data.Angle4,
                Angle5 = data.Angle5,
                Angle6 = data.Angle6,
                MaxSteeringAngle = data.MaxSteeringAngle,
                Unknown2 = data.Unknown2
            };
    }
}