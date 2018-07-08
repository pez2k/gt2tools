using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class TyresRear : CarCsvDataStructure<TyresRearData, TyresRearCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            string filename = Name + "\\" + Data.CarId.ToCarName();

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }
            
            return filename + "\\" + Utils.TyreStageConverter.ConvertToString(Data.Stage, null, null) + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x0C
    public struct TyresRearData
    {
        public uint CarId;
        public byte Stage;
        public byte SteeringReaction1;
        public byte WheelSize;
        public byte SteeringReaction2;
        public byte TyreCompound;
        public byte Unknown1;
        public byte Unknown2;
        public byte Unknown3;
    }

    public sealed class TyresRearCSVMap : ClassMap<TyresRearData>
    {
        public TyresRearCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Stage).TypeConverter(Utils.TyreStageConverter);
            Map(m => m.SteeringReaction1);
            Map(m => m.WheelSize);
            Map(m => m.SteeringReaction2);
            Map(m => m.TyreCompound).TypeConverter(Utils.TyreCompoundConverter);
            Map(m => m.Unknown1);
            Map(m => m.Unknown2);
            Map(m => m.Unknown3);
        }
    }
}