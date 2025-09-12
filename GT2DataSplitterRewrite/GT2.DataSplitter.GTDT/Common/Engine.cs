using System.Runtime.InteropServices;

namespace GT2.DataSplitter.GTDT.Common
{
    using CarNameConversion;

    public class Engine : MappedDataStructure<Engine.Data>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x4C
        public struct Data
        {
            public uint CarId; // 0
            public ushort LayoutName; // V6 etc, index into unistringdb, 0x4
            public ushort ValvetrainName; // DOHC etc, index into unistringdb, 0x6
            public ushort Aspiration; // 
            public ushort SoundFile; // 
            public short TorqueCurve1; // 0xc - all these take part in garage HP calculation
            public short TorqueCurve2;
            public short TorqueCurve3;
            public short TorqueCurve4;
            public short TorqueCurve5;
            public short TorqueCurve6;
            public short TorqueCurve7;
            public short TorqueCurve8;
            public short TorqueCurve9;
            public short TorqueCurve10;
            public short TorqueCurve11;
            public short TorqueCurve12;
            public short TorqueCurve13;
            public short TorqueCurve14;
            public short TorqueCurve15;
            public short TorqueCurve16;
            public ushort Displacement; // 0x2c
            public ushort DisplayedPower; // 0x2e - base ps. F. Ex. is 295hp for most of the rally cars, not 400+
            public ushort MaxPowerRPM; // 0x30 - multipled by 10 by the game. E.g. a value of 850 is displayed as 8500
            public ushort DisplayedTorque; // 0x32 - divided by 10 by the game. e.g. a value of 950 is displayed as 95.0
            public ushort MaxTorqueRPMName; // 0x34 - index into unistringdb!
            public byte PowerMultiplier; // 0x36 - used in hp calculation
            public byte ClutchReleaseRPM;
            public byte IdleRPM;
            public byte MaxRPM;
            public byte RedlineRPM;
            public byte TorqueCurveRPM1; // 0x3b - rpms that the bandAcceleration values are for
            public byte TorqueCurveRPM2;
            public byte TorqueCurveRPM3;
            public byte TorqueCurveRPM4;
            public byte TorqueCurveRPM5;
            public byte TorqueCurveRPM6;
            public byte TorqueCurveRPM7;
            public byte TorqueCurveRPM8;
            public byte TorqueCurveRPM9;
            public byte TorqueCurveRPM10;
            public byte TorqueCurveRPM11;
            public byte TorqueCurveRPM12;
            public byte TorqueCurveRPM13;
            public byte TorqueCurveRPM14;
            public byte TorqueCurveRPM15;
            public byte TorqueCurveRPM16;
            public byte TorqueCurvePoints; // the number of values in both arrays that are used in hp calculations
        }

        public Models.Common.Engine MapToModel(UnicodeStringTable strings) =>
            new Models.Common.Engine
            {
                CarId = data.CarId.ToCarName(),
                LayoutName = strings.Get(data.LayoutName),
                ValvetrainName = strings.Get(data.ValvetrainName),
                Aspiration = strings.Get(data.Aspiration),
                SoundFile = data.SoundFile,
                TorqueCurve1 = data.TorqueCurve1,
                TorqueCurve2 = data.TorqueCurve2,
                TorqueCurve3 = data.TorqueCurve3,
                TorqueCurve4 = data.TorqueCurve4,
                TorqueCurve5 = data.TorqueCurve5,
                TorqueCurve6 = data.TorqueCurve6,
                TorqueCurve7 = data.TorqueCurve7,
                TorqueCurve8 = data.TorqueCurve8,
                TorqueCurve9 = data.TorqueCurve9,
                TorqueCurve10 = data.TorqueCurve10,
                TorqueCurve11 = data.TorqueCurve11,
                TorqueCurve12 = data.TorqueCurve12,
                TorqueCurve13 = data.TorqueCurve13,
                TorqueCurve14 = data.TorqueCurve14,
                TorqueCurve15 = data.TorqueCurve15,
                TorqueCurve16 = data.TorqueCurve16,
                Displacement = ToDisplacementString(data.Displacement),
                DisplayedPower = data.DisplayedPower,
                MaxPowerRPM = data.MaxPowerRPM,
                DisplayedTorque = data.DisplayedTorque,
                MaxTorqueRPMName = strings.Get(data.MaxTorqueRPMName),
                PowerMultiplier = data.PowerMultiplier,
                ClutchReleaseRPM = data.ClutchReleaseRPM,
                IdleRPM = data.IdleRPM,
                MaxRPM = data.MaxRPM,
                RedlineRPM = data.RedlineRPM,
                TorqueCurveRPM1 = data.TorqueCurveRPM1,
                TorqueCurveRPM2 = data.TorqueCurveRPM2,
                TorqueCurveRPM3 = data.TorqueCurveRPM3,
                TorqueCurveRPM4 = data.TorqueCurveRPM4,
                TorqueCurveRPM5 = data.TorqueCurveRPM5,
                TorqueCurveRPM6 = data.TorqueCurveRPM6,
                TorqueCurveRPM7 = data.TorqueCurveRPM7,
                TorqueCurveRPM8 = data.TorqueCurveRPM8,
                TorqueCurveRPM9 = data.TorqueCurveRPM9,
                TorqueCurveRPM10 = data.TorqueCurveRPM10,
                TorqueCurveRPM11 = data.TorqueCurveRPM11,
                TorqueCurveRPM12 = data.TorqueCurveRPM12,
                TorqueCurveRPM13 = data.TorqueCurveRPM13,
                TorqueCurveRPM14 = data.TorqueCurveRPM14,
                TorqueCurveRPM15 = data.TorqueCurveRPM15,
                TorqueCurveRPM16 = data.TorqueCurveRPM16,
                TorqueCurvePoints = data.TorqueCurvePoints
            };

        private static string ToDisplacementString(ushort displacement)
        {
            const ushort TimesTwo = 0x4000;
            const ushort TimesThree = 0x6000;

            string suffix = "";
            if ((displacement & TimesThree) == TimesThree)
            {
                displacement ^= TimesThree;
                suffix = "x3";
            }
            if ((displacement & TimesTwo) == TimesTwo)
            {
                displacement ^= TimesTwo;
                suffix = "x2";
            }

            return $"{displacement}{suffix}";
        }
    }
}