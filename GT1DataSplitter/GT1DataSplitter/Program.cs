using System.Globalization;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    class Program
    {
        public static CsvConfiguration CSVConfig => new(CultureInfo.CurrentUICulture) { ShouldQuote = (args) => true };

        public static string LanguagePrefix { get; private set; }

        public static void Main(string[] args)
        {
            DumpDataFile<CarInfData>("CARINF.DAT", false);
            if (args.Length != 1)
            {
                //BuildDataFile();
                return;
            }
            DumpDataFile<CarInfData>("CARINF.DAT", false);
        }

        private static void DumpDataFile<TData>(string filename, bool favourCompressed) where TData : DataFile, new()
        {
            TData data = new();
            data.ReadData(filename);
            data.DumpData();
        }
    }
}