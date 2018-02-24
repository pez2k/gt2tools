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

        static CarDataStructure[] dataStructures =
        {
            new Brakes(), new BrakeBalanceController(), new Steering(), new Dimensions(),  new WeightReduction(), new Body(), new Engine(), new PortPolishing(),
            new EngineBalancing(), new DisplacementIncrease(), new Chip(), new NATuning(), new TurboKit(), new Drivetrain(), new Flywheel(), new Clutch(),
            new Propshaft(), new Transmission(), new Suspension(), new Intercooler(), new Exhaust(), new Differential(), new TyresFront(), new TyresRear(),
            new CarUnknown1(), new CarUnknown2(), new CarUnknown3(), new CarUnknown4(), new CarUnknown5(), new CarUnknown6(), new Car()
        };

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
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                file.Write(new byte[]{ 0x47, 0x54, 0x44, 0x54, 0x6C, 0x00, 0x3E, 0x00 }, 0, 8);

                file.Position = 0x1F7;
                file.WriteByte(0x00); // Data starts at 0x1F8 so position EOF
                uint i = 1;
                foreach (CarDataStructure dataStructure in dataStructures)
                {
                    dataStructure.WriteData(file, 8 * i);
                    i++;
                }

                file.Position = 0;
                using (FileStream zipFile = new FileStream(filename + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new GZipStream(zipFile, CompressionMode.Compress))
                    {
                        file.CopyTo(zip);
                    }
                }
            }
        }
    }
}
