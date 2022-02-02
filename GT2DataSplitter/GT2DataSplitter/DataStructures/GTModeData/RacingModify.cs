using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class RacingModify : CarCsvDataStructure<RacingModifyData, RacingModifyCSVMap>
    {
        protected override string CreateOutputFilename()
        {
            string filename = Name + "\\" + data.CarId.ToCarName();
            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }
            string number = Directory.GetFiles(filename).Length.ToString();
            return filename + "\\" + number + "_stage" + data.Stage.ToString() + "_" + data.BodyId.ToCarName() + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x1C
    public struct RacingModifyData
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

    public sealed class RacingModifyCSVMap : ClassMap<RacingModifyData>
    {
        public RacingModifyCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.BodyId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Weight);
            Map(m => m.BodyRollAmount);
            Map(m => m.Stage);
            Map(m => m.Drag);
            Map(m => m.FrontDownforceMinimum);
            Map(m => m.FrontDownforceMaximum);
            Map(m => m.FrontDownforceDefault);
            Map(m => m.RearDownforceMinimum);
            Map(m => m.RearDownforceMaximum);
            Map(m => m.RearDownforceDefault);
            Map(m => m.Unknown3);
            Map(m => m.Unknown4);
            Map(m => m.Unknown5);
            Map(m => m.Unknown6);
            Map(m => m.Width);
        }
    }
}