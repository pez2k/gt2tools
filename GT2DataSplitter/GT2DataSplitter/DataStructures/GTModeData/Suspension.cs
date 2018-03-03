using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT2DataSplitter
{
    public class Suspension : CarCsvDataStructure<SuspensionData, SuspensionCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            return CreateOutputFilename(Data.CarId, Data.Stage);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x4C
    public struct SuspensionData
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
        public byte Unknown;
        public byte Unknown2;
        public byte Unknown3;
        public byte Unknown4;
        public byte MinSpringRateFront;
        public byte MaxSpringRateFront;
        public byte DefaultSpringRateFront;
        public byte MinSpringRateRear;
        public byte MaxSpringRateRear;
        public byte DefaultSpringRateRear;
        public byte Unknown5;
        public byte Unknown6;
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
        public byte MaxStabiliserFront;
        public byte Unknown29;
        public byte Unknown30;
        public byte DefaultStabiliserFront;
        public byte MaxStabiliserRear;
        public byte Unknown31;
        public byte Unknown32;
        public byte DefaultStabiliserRear;
        public byte Unknown33;
    }

    public sealed class SuspensionCSVMap : ClassMap<SuspensionData>
    {
        public SuspensionCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage);
            Map(m => m.MinCamberFront);
            Map(m => m.MaxCamberFront);
            Map(m => m.DefaultCamberFront);
            Map(m => m.MinCamberRear);
            Map(m => m.MaxCamberRear);
            Map(m => m.DefaultCamberRear);
            Map(m => m.MinToeFront);
            Map(m => m.MaxToeFront);
            Map(m => m.MinToeRear);
            Map(m => m.MaxToeRear);
            Map(m => m.MinHeightFront);
            Map(m => m.MaxHeightFront);
            Map(m => m.DefaultHeightFront);
            Map(m => m.MinHeightRear);
            Map(m => m.MaxHeightRear);
            Map(m => m.DefaultHeightRear);
            Map(m => m.Unknown);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.MinSpringRateFront);
            Map(m => m.MaxSpringRateFront);
            Map(m => m.DefaultSpringRateFront);
            Map(m => m.MinSpringRateRear);
            Map(m => m.MaxSpringRateRear);
            Map(m => m.DefaultSpringRateRear);
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.Unknown8);
            Map(m => m.MaxDamperBoundFront);
            Map(m => m.Unknown9);
            Map(m => m.Unknown10);
            Map(m => m.DefaultDamperBoundFront);
            Map(m => m.Unknown11);
            Map(m => m.Unknown12);
            Map(m => m.Unknown13);
            Map(m => m.MaxDamperReboundFront);
            Map(m => m.Unknown14);
            Map(m => m.Unknown15);
            Map(m => m.DefaultDamperReboundFront);
            Map(m => m.Unknown16);
            Map(m => m.Unknown17);
            Map(m => m.Unknown18);
            Map(m => m.MaxDamperBoundRear);
            Map(m => m.Unknown19);
            Map(m => m.Unknown20);
            Map(m => m.DefaultDamperBoundRear);
            Map(m => m.Unknown21);
            Map(m => m.Unknown22);
            Map(m => m.Unknown23);
            Map(m => m.MaxDamperReboundRear);
            Map(m => m.Unknown24);
            Map(m => m.Unknown25);
            Map(m => m.DefaultDamperReboundRear);
            Map(m => m.Unknown26);
            Map(m => m.Unknown27);
            Map(m => m.Unknown28);
            Map(m => m.MaxStabiliserFront);
            Map(m => m.Unknown29);
            Map(m => m.Unknown30);
            Map(m => m.DefaultStabiliserFront);
            Map(m => m.MaxStabiliserRear);
            Map(m => m.Unknown31);
            Map(m => m.Unknown32);
            Map(m => m.DefaultStabiliserRear);
            Map(m => m.Unknown33);
        }
    }
}
