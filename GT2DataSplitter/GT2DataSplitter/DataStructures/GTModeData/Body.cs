using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class Body : CarCsvDataStructure<BodyData, BodyCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            string filename = Name;
            
            filename += "\\" + Data.CarId.ToCarName();

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }

            string number = Directory.GetFiles(filename).Length.ToString();

            return filename + "\\" + number + "_stage" + Data.Stage.ToString() + "_" + Data.BodyId.ToCarName() + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x1C
    public struct BodyData
    {
        public uint CarId;
        public uint Price; // if 0 or low byte = 0, not possible
        public uint BodyId; // e.g. if car id ends with 0x8, rmCarId will end with 0xc, d, e etc
        public byte Weight; // weight is a multiple of some car-indepenent value
        public byte BodyRollAmount;
        public byte Stage;
        public byte Unknown2;
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

    public sealed class BodyCSVMap : ClassMap<BodyData>
    {
        public BodyCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.BodyId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Weight);
            Map(m => m.BodyRollAmount);
            Map(m => m.Stage);
            Map(m => m.Unknown2);
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
