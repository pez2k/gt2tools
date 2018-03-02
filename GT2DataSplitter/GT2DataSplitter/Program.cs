using System.IO;
using System.IO.Compression;

namespace GT2DataSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                BuildFile("eng_gtmode_data.dat");
                return;
            }

            string filename = args[0];
            string extension = Path.GetExtension(filename);
            
            if (extension == ".gz")
            {
                string innerFilename = Path.GetFileNameWithoutExtension(filename);
                extension = Path.GetExtension(innerFilename);

                if (extension != ".dat")
                {
                    return;
                }

                using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    // Un-gzip
                    using (GZipStream unzip = new GZipStream(infile, CompressionMode.Decompress))
                    {
                        filename = innerFilename;
                        using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                        {
                            unzip.CopyTo(outfile);
                        }
                    }
                }
            }

            if (extension == ".dat")
            {
                SplitFile(filename);
            }
        }

        static void SplitFile(string filename)
        {
            GTModeData CarData = new GTModeData();
            CarData.ReadData(filename);
            CarData.DumpData();

            GTModeRace RaceData = new GTModeRace();
            RaceData.ReadData("eng_gtmode_race.dat");
            RaceData.DumpData();
        }

        static void BuildFile(string filename)
        {
            GTModeData CarData = new GTModeData();
            CarData.ImportData();
            CarData.WriteData(filename);

            GTModeRace RaceData = new GTModeRace();
            RaceData.ImportData();
            RaceData.WriteData("eng_gtmode_race.dat");
        }
    }
}
