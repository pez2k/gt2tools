namespace GT2.DataSplitter
{
    using GTDT;
    using Models;

    internal class Program
    {
        static void Main(string[] args)
        {
            // DataFile data = new GameFileDataReader().Read("eng_gtmode_data.dat.gz");
            // new CSVDataWriter().Write(data, "/");

            GTModeModel model = GTDTReader.ReadGTMode("eng");

            ArcadeModel arcade = GTDTReader.ReadArcade("eng");

            LicenseModel license = GTDTReader.ReadLicense("eng");
        }
    }
}
