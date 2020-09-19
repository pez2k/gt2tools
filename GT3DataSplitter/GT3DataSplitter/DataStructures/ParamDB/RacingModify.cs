using CsvHelper.Configuration;
using System.Runtime.InteropServices;

namespace GT3.DataSplitter
{
    public class RacingModify : CsvDataStructure<RacingModifyData, RacingModifyCSVMap>
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x48
    public struct RacingModifyData
    {
        public ulong Part;
        public ulong Car;
        public byte Stage;
        public byte FrontDownforceMin;
        public byte FrontDownforceMax;
        public byte FrontDownforceDefault;
        public uint PriceMaybe;
        public ulong Filename;
        public byte Unknown1;
        public byte Unknown2;
        public byte Unknown3;
        public byte Unknown4;
        public byte Unknown5;
        public byte Unknown6;
        public byte Unknown7;
        public byte Unknown8;
        public byte Unknown9;
        public byte Unknown10;
        public byte Unknown11;
        public byte Unknown12;
        public byte Unknown13;
        public byte Unknown14;
        public byte Unknown15;
        public byte Unknown16;
        public ushort Unknown17;
        public ushort FrontTrack;
        public ushort RearTrack;
        public ushort Width;
        public byte Unknown18;
        public byte RearDownforceMin;
        public byte RearDownforceMax;
        public byte RearDownforceDefault;
        public byte Unknown19;
        public byte Unknown20;
        public byte Unknown21;
        public byte Unknown22;
        public byte Unknown23;
        public byte Unknown24;
        public byte Unknown25;
        public byte Unknown26;
        public byte Unknown27;
        public byte Unknown28;
        public byte Unknown29;
        public byte Unknown30;
    }

    public sealed class RacingModifyCSVMap : ClassMap<RacingModifyData>
    {
        public RacingModifyCSVMap()
        {
            Map(m => m.Part).TypeConverter(Utils.IdConverter);
            Map(m => m.Car).TypeConverter(Utils.IdConverter);
            Map(m => m.Stage);
            Map(m => m.FrontDownforceMin);
            Map(m => m.FrontDownforceMax);
            Map(m => m.FrontDownforceDefault);
            Map(m => m.PriceMaybe);
            Map(m => m.Filename).TypeConverter(Utils.IdConverter);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
            Map(m => m.Unknown7);
            Map(m => m.Unknown8);
            Map(m => m.Unknown9);
            Map(m => m.Unknown10);
            Map(m => m.Unknown11);
            Map(m => m.Unknown12);
            Map(m => m.Unknown13);
            Map(m => m.Unknown14);
            Map(m => m.Unknown15);
            Map(m => m.Unknown16);
            Map(m => m.Unknown17);
            Map(m => m.FrontTrack);
            Map(m => m.RearTrack);
            Map(m => m.Width);
            Map(m => m.Unknown18);
            Map(m => m.RearDownforceMin);
            Map(m => m.RearDownforceMax);
            Map(m => m.RearDownforceDefault);
            Map(m => m.Unknown19);
            Map(m => m.Unknown20);
            Map(m => m.Unknown21);
            Map(m => m.Unknown22);
            Map(m => m.Unknown23);
            Map(m => m.Unknown24);
            Map(m => m.Unknown25);
            Map(m => m.Unknown26);
            Map(m => m.Unknown27);
            Map(m => m.Unknown28);
            Map(m => m.Unknown29);
            Map(m => m.Unknown30);
        }
    }
}