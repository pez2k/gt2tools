using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GT2.DataSplitter.TypeConverters
{
    public class CachedFileNameConverter : ITypeConverter
    {
        public virtual string Name { get; set; }

        public CachedFileNameConverter(string name = "")
        {
            Name = name;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            int stringNumber = FileNameCache.Get(Name, text);
            if (Name == nameof(Regulations) || Name == nameof(TireSize) || Name == nameof(TireCompound))
            {
                return (byte)stringNumber;
            }
            else if (Name == nameof(EnemyCars) || Name == nameof(EnemyCarsArcade))
            {
                return (uint)stringNumber;
            }
            else
            {
                return (ushort)stringNumber;
            }
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            int cacheIndex = int.Parse(value.ToString());
            return FileNameCache.Get(Name, cacheIndex);
        }
    }
}