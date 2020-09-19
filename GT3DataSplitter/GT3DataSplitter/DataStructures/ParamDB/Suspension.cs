using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class Suspension : CsvDataStructure<SuspensionData, SuspensionCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x80
    public struct SuspensionData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte MinCamberFront;
        public byte MaxCamberFront;
        public byte DefaultCamberFront;
        public byte MinCamberRear;
        public byte MaxCamberRear;
        public byte DefaultCamberRear;
        public byte MinToeFront;
        public byte MaxToeFront;
        public byte DefaultToeFront;
        public byte MinToeRear;
        public byte MaxToeRear;
        public byte DefaultToeRear;
        public byte Unknown1; // lower with upgrades
        public byte Unknown2; // lower with upgrades
        public byte Unknown3; // lower with upgrades
        public byte Unknown4; // lower with upgrades
        public byte MinSpringRateFront;
        public byte MaxSpringRateFront;
        public byte DefaultSpringRateFront;
        public byte MinSpringRateRear;
        public byte MaxSpringRateRear;
        public byte DefaultSpringRateRear;
        public byte FrontLeverRatioDefault;
        public byte RearLeverRatioDefault;
        public byte Unknown5; // same for all levels per car
        public byte Unknown6; // same for all levels per car
        public byte FrontDamperBoundSteps;
        public byte FrontDamperBoundMinValue;
        public byte FrontDamperBoundMaxValue;
        public byte FrontDamperBoundDefaultStep;
        public byte FrontDamperUnknown1MinValue;
        public byte FrontDamperUnknown1MaxValue;
        public byte FrontDamperUnknown1DefaultStep;
        public byte FrontDamperReboundSteps;
        public byte FrontDamperReboundMinValue;
        public byte FrontDamperReboundMaxValue;
        public byte FrontDamperReboundDefaultStep;
        public byte FrontDamperUnknown2MinValue;
        public byte FrontDamperUnknown2MaxValue;
        public byte FrontDamperUnknown2DefaultStep;
        public byte RearDamperBoundSteps;
        public byte RearDamperBoundMinValue;
        public byte RearDamperBoundMaxValue;
        public byte RearDamperBoundDefaultStep;
        public byte RearDamperUnknown1MinValue;
        public byte RearDamperUnknown1MaxValue;
        public byte RearDamperUnknown1DefaultStep;
        public byte RearDamperReboundSteps;
        public byte RearDamperReboundMinValue;
        public byte RearDamperReboundMaxValue;
        public byte RearDamperReboundDefaultStep;
        public byte RearDamperUnknown2MinValue;
        public byte RearDamperUnknown2MaxValue;
        public byte RearDamperUnknown2DefaultStep;
        public byte FrontStabiliserSteps;
        public byte FrontStabiliserMinValue;
        public byte FrontStabiliserMaxValue;
        public byte FrontStabiliserDefaultStep;
        public byte RearStabiliserSteps;
        public byte RearStabiliserMinValue;
        public byte RearStabiliserMaxValue;
        public byte RearStabiliserDefaultStep;
        public byte Unknown7; // same for all levels per car
        public ushort MinHeightFront;
        public ushort MaxHeightFront;
        public ushort DefaultHeightFront;
        public ushort MinHeightRear;
        public ushort MaxHeightRear;
        public ushort DefaultHeightRear;
        public byte Unknown8; // same for all levels per car
        public byte Unknown9; // same for all levels per car
        public byte Unknown10; // same for all levels per car
        public byte Unknown11; // same for all levels per car
        public byte FrontCamberGripX1;
        public byte FrontCamberGripX2;
        public byte FrontCamberGripX3;
        public byte FrontCamberGripX4;
        public byte FrontCamberGripY1;
        public byte FrontCamberGripY2;
        public byte FrontCamberGripY3;
        public byte FrontCamberGripY4;
        public byte RearCamberGripX1;
        public byte RearCamberGripX2;
        public byte RearCamberGripX3;
        public byte RearCamberGripX4;
        public byte RearCamberGripY1;
        public byte RearCamberGripY2;
        public byte RearCamberGripY3;
        public byte RearCamberGripY4;
        public byte FrontDamperV1Bound;
        public byte FrontDamperV2Bound;
        public byte FrontDamperV1Rebound;
        public byte FrontDamperV2Rebound;
        public byte RearDamperV1Bound;
        public byte RearDamperV2Bound;
        public byte RearDamperV1Rebound;
        public byte RearDamperV2Rebound;
        public byte Unknown12; // same for all levels per car
        public byte Unknown13; // same for all levels per car
        public byte Unknown14; // same for all levels per car
        public byte Unknown15; // same for all levels per car
        public uint Price;
    }

    public sealed class SuspensionCSVMap : ClassMap<SuspensionData>
    {
        public SuspensionCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.MinCamberFront);
            Map(m => m.MaxCamberFront);
            Map(m => m.DefaultCamberFront);
            Map(m => m.MinCamberRear);
            Map(m => m.MaxCamberRear);
            Map(m => m.DefaultCamberRear);
            Map(m => m.MinToeFront);
            Map(m => m.MaxToeFront);
            Map(m => m.DefaultToeFront);
            Map(m => m.MinToeRear);
            Map(m => m.MaxToeRear);
            Map(m => m.DefaultToeRear);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.MinSpringRateFront);
            Map(m => m.MaxSpringRateFront);
            Map(m => m.DefaultSpringRateFront);
            Map(m => m.MinSpringRateRear);
            Map(m => m.MaxSpringRateRear);
            Map(m => m.DefaultSpringRateRear);
            Map(m => m.FrontLeverRatioDefault);
            Map(m => m.RearLeverRatioDefault);
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
            Map(m => m.FrontDamperBoundSteps);
            Map(m => m.FrontDamperBoundMinValue);
            Map(m => m.FrontDamperBoundMaxValue);
            Map(m => m.FrontDamperBoundDefaultStep);
            Map(m => m.FrontDamperUnknown1MinValue);
            Map(m => m.FrontDamperUnknown1MaxValue);
            Map(m => m.FrontDamperUnknown1DefaultStep);
            Map(m => m.FrontDamperReboundSteps);
            Map(m => m.FrontDamperReboundMinValue);
            Map(m => m.FrontDamperReboundMaxValue);
            Map(m => m.FrontDamperReboundDefaultStep);
            Map(m => m.FrontDamperUnknown2MinValue);
            Map(m => m.FrontDamperUnknown2MaxValue);
            Map(m => m.FrontDamperUnknown2DefaultStep);
            Map(m => m.RearDamperBoundSteps);
            Map(m => m.RearDamperBoundMinValue);
            Map(m => m.RearDamperBoundMaxValue);
            Map(m => m.RearDamperBoundDefaultStep);
            Map(m => m.RearDamperUnknown1MinValue);
            Map(m => m.RearDamperUnknown1MaxValue);
            Map(m => m.RearDamperUnknown1DefaultStep);
            Map(m => m.RearDamperReboundSteps);
            Map(m => m.RearDamperReboundMinValue);
            Map(m => m.RearDamperReboundMaxValue);
            Map(m => m.RearDamperReboundDefaultStep);
            Map(m => m.RearDamperUnknown2MinValue);
            Map(m => m.RearDamperUnknown2MaxValue);
            Map(m => m.RearDamperUnknown2DefaultStep);
            Map(m => m.FrontStabiliserSteps);
            Map(m => m.FrontStabiliserMinValue);
            Map(m => m.FrontStabiliserMaxValue);
            Map(m => m.FrontStabiliserDefaultStep);
            Map(m => m.RearStabiliserSteps);
            Map(m => m.RearStabiliserMinValue);
            Map(m => m.RearStabiliserMaxValue);
            Map(m => m.RearStabiliserDefaultStep);
            Map(m => m.Unknown7);
            Map(m => m.MinHeightFront);
            Map(m => m.MaxHeightFront);
            Map(m => m.DefaultHeightFront);
            Map(m => m.MinHeightRear);
            Map(m => m.MaxHeightRear);
            Map(m => m.DefaultHeightRear);
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.Unknown10);
            Map(m => m.Unknown11);
            Map(m => m.FrontCamberGripX1);
            Map(m => m.FrontCamberGripX2);
            Map(m => m.FrontCamberGripX3);
            Map(m => m.FrontCamberGripX4);
            Map(m => m.FrontCamberGripY1);
            Map(m => m.FrontCamberGripY2);
            Map(m => m.FrontCamberGripY3);
            Map(m => m.FrontCamberGripY4);
            Map(m => m.RearCamberGripX1);
            Map(m => m.RearCamberGripX2);
            Map(m => m.RearCamberGripX3);
            Map(m => m.RearCamberGripX4);
            Map(m => m.RearCamberGripY1);
            Map(m => m.RearCamberGripY2);
            Map(m => m.RearCamberGripY3);
            Map(m => m.RearCamberGripY4);
            Map(m => m.FrontDamperV1Bound);
            Map(m => m.FrontDamperV2Bound);
            Map(m => m.FrontDamperV1Rebound);
            Map(m => m.FrontDamperV2Rebound);
            Map(m => m.RearDamperV1Bound);
            Map(m => m.RearDamperV2Bound);
            Map(m => m.RearDamperV1Rebound);
            Map(m => m.RearDamperV2Rebound);
            Map(m => m.Unknown12);
            Map(m => m.Unknown13);
            Map(m => m.Unknown14);
            Map(m => m.Unknown15);
            Map(m => m.Price);
        }
    }
}