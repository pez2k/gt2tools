using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    class Program
    {
        public static CsvConfiguration CSVConfig => new(CultureInfo.CurrentUICulture) { ShouldQuote = (args) => true };

        public static string LanguagePrefix { get; private set; }

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Required to support code pages, including 932

            if (args.Length != 1)
            {
                BuildDataFile<CarInfData>();
                return;
            }
            DumpDataFile<CarInfData>(args[0]);
        }

        private static void DumpDataFile<TData>(string filename) where TData : DataFile, new()
        {
            TData data = new();
            data.ReadData(filename);
            data.DumpData();
        }

        private static void BuildDataFile<TData>() where TData : DataFile, new()
        {
            TData data = new();
            data.ImportData();
            Directory.CreateDirectory("Output");
            data.WriteData(Path.Combine("Output", "CARINF.DAT"));
        }
    }
}