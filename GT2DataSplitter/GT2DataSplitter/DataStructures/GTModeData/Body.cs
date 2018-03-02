using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2DataSplitter
{
    public class Body : CsvDataStructure
    {
        public Body()
        {
            Size = 0x1C;
            CacheFilename = true;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            string filename = Name;
            
            filename += "\\" + Utils.GetCarName(Data.CarId);

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }

            string number = Directory.GetFiles(filename).Length.ToString();

            return filename + "\\" + number + "_stage" + Data.Stage.ToString() + "_" + Utils.GetCarName(Data.BodyId) + ".csv";
        }

        public override void Read(FileStream infile)
        {
            Data = ReadStructure<StructureData>(infile);
        }

        public override void Dump()
        {
            DumpCsv<StructureData, BodyCSVMap>(Data);
        }

        public override void Import(string filename)
        {
            Data = ImportCsv<StructureData, BodyCSVMap>(filename);
        }

        public override void Write(FileStream outfile)
        {
            WriteStructure(outfile, Data);
        }

        public StructureData Data { get; set; }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public new class StructureData : CsvDataStructure.StructureData
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
    }

    public sealed class BodyCSVMap : ClassMap<Body.StructureData>
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
