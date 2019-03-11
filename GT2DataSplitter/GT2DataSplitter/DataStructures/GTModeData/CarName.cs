using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    public class CarName
    {
        public uint CarID { get; set; }
        public string NameFirstPart { get; set; }
        public string NameSecondPart { get; set; }
        public byte Year { get; set; }
    }

    public sealed class CarNameCSVMap : ClassMap<CarName>
    {
        public CarNameCSVMap()
        {
            Map(m => m.CarID).TypeConverter(Utils.CarIdConverter);
            Map(m => m.NameFirstPart);
            Map(m => m.NameSecondPart);
            Map(m => m.Year);
        }
    }
}