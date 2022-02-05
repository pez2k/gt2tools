using System.IO;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using CarNameConversion;
    using TypeConverters;

    public class TiresRear : CarCsvDataStructure<TiresRearData, TiresRearCSVMap>
    {
        protected override string CreateOutputFilename()
        {
            string filename = Name + "\\" + data.CarId.ToCarName();
            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }
            string number = Directory.GetFiles(filename).Length.ToString();
            return filename + "\\" + number + "_" + new TireStageConverter().ConvertToString(data.Stage, null, null) + ".csv";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 0x0C
    public struct TiresRearData
    {
        public uint CarId;
        public byte Stage;
        public byte SteeringReaction1;
        public byte WheelSize;
        public byte SteeringReaction2;
        public byte TireCompound;
        public byte TireForceVolMaybe;
        public byte SlipMultiplier;
        public byte GripMultiplier;
    }

    public sealed class TiresRearCSVMap : ClassMap<TiresRearData>
    {
        public TiresRearCSVMap()
        {
            Map(m => m.CarId).CarId();
            Map(m => m.Stage).TypeConverter(new TireStageConverter());
            Map(m => m.SteeringReaction1);
            Map(m => m.WheelSize).PartFilename(nameof(TireSize));
            Map(m => m.SteeringReaction2);
            Map(m => m.TireCompound).PartFilename(nameof(TireCompound));
            Map(m => m.TireForceVolMaybe);
            Map(m => m.SlipMultiplier);
            Map(m => m.GripMultiplier);
        }
    }
}