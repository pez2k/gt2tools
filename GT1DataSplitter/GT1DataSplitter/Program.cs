using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;

namespace GT1.DataSplitter
{
    using Caches;

    class Program
    {
        public static CsvConfiguration CSVConfig => new(CultureInfo.CurrentUICulture) { ShouldQuote = (args) => true };

        public static string LanguagePrefix { get; private set; }

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Required to support code pages, including 932

            int windowSize = 0;
            if (args.Length == 1 && int.TryParse(args[0], out int compressionLevel) && compressionLevel >= -1 && compressionLevel <= 32)
            {
                windowSize = compressionLevel * 1024;
                BuildDataFile<CarInfData>(windowSize);
                return;
            }

            if (args.Length != 1)
            {
                BuildDataFile<CarInfData>(windowSize);
                return;
            }
            DumpDataFile<CarInfData>(args[0]);
        }

        private static void DumpDataFile<TData>(string filename) where TData : DataFile, new()
        {
            TData data = new();
            data.ReadData(filename);
            data.DumpData();

            using (StreamWriter ids = File.CreateText("_ids.txt"))
            {
                foreach (string id in CarIDCache.Cache.Skip(1))
                {
                    ids.WriteLine(id);
                }
            }
        }

        private static void BuildDataFile<TData>(int windowSize) where TData : DataFile, new()
        {
            using (StreamReader ids = File.OpenText("_ids.txt"))
            {
                while (!ids.EndOfStream)
                {
                    CarIDCache.Add(ids.ReadLine());
                }
            }
            TData data = new();
            data.ImportData();
            Directory.CreateDirectory("Output");
            data.WriteData(Path.Combine("Output", "CARINF.DAT"), windowSize);
        }
    }
}