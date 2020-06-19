using CsvHelper.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace GT2.DataSplitter
{
    using CarNameConversion;

    public class TiresFront : CarCsvDataStructure<TiresFrontData, TiresFrontCSVMap>
    {
        public override string CreateOutputFilename(byte[] data)
        {
            string filename = Name + "\\" + Data.CarId.ToCarName();

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }

            string number = Directory.GetFiles(filename).Length.ToString();
            return filename + "\\" + number + "_" + Utils.TireStageConverter.ConvertToString(Data.Stage, null, null) + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x10
    public struct TiresFrontData
    {
        public uint CarId;
        public uint Price;
        public byte Stage;
        public byte SteeringReaction1;
        public byte WheelSize;
        public byte SteeringReaction2;
        public byte TireCompound;
        public byte Unknown1;
        public byte SlipMultiplier;
        public byte GripMultiplier;
    }

    public sealed class TiresFrontCSVMap : ClassMap<TiresFrontData>
    {
        public TiresFrontCSVMap()
        {
            Map(m => m.CarId).TypeConverter(Utils.CarIdConverter);
            Map(m => m.Price);
            Map(m => m.Stage).TypeConverter(Utils.TireStageConverter);
            Map(m => m.SteeringReaction1);
            Map(m => m.WheelSize).TypeConverter(Utils.GetFileNameConverter(nameof(TireSize)));
            Map(m => m.SteeringReaction2);
            Map(m => m.TireCompound).TypeConverter(Utils.TireCompoundConverter);
            Map(m => m.Unknown1);
            Map(m => m.SlipMultiplier);
            Map(m => m.GripMultiplier);
        }
    }
}
