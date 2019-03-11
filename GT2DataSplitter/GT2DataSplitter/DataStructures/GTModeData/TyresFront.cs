using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class TyresFront : CarCsvDataStructure<TyresFrontData, TyresFrontCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            string filename = Name + "\\" + Data.CarId.ToCarName();

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }

            string number = Directory.GetFiles(filename).Length.ToString();
            return filename + "\\" + number + "_" + Utils.TyreStageConverter.ConvertToString(Data.Stage, null, null) + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
    public struct TyresFrontData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte SteeringReaction1;
        public byte WheelSize;
        public byte SteeringReaction2;
        public byte TyreCompound;
        public byte Unknown1;
        public byte SlipMultiplier;
        public byte GripMultiplier;
    }

    public sealed class TyresFrontCSVMap : ClassMap<TyresFrontData>
    {
        public TyresFrontCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage).TypeConverter(Utils.TyreStageConverter);
            Map(m => m.SteeringReaction1);
            Map(m => m.WheelSize);
            Map(m => m.SteeringReaction2);
            Map(m => m.TyreCompound).TypeConverter(Utils.TyreCompoundConverter);
            Map(m => m.Unknown1);
            Map(m => m.SlipMultiplier);
            Map(m => m.GripMultiplier);
        }
    }
}
