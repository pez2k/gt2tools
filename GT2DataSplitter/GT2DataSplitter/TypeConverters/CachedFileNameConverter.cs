using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class CachedFileNameConverter : ITypeConverter
    {
        private readonly string name;

        public CachedFileNameConverter(string name) => this.name = name;

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            int stringNumber = FileNameCache.Get(name, text);
            if (name == nameof(Regulations) || name == nameof(TireSize) || name == nameof(TireCompound))
            {
                return (byte)stringNumber;
            }
            else if (name == nameof(EnemyCars) || name == nameof(EnemyCarsArcade))
            {
                return (uint)stringNumber;
            }
            else
            {
                return (ushort)stringNumber;
            }
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) =>
            FileNameCache.Get(name, int.Parse(value.ToString()));
    }
}