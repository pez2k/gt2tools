using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace GT2.ArcadeStatsEditor
{
    using StreamExtensions;

    class Program
    {
        private const int CarIDs = 0x170E0;
        private const int StatsBase = 0x41B20;
        private const int CarCounts = StatsBase;
        private const int ClassSBase = StatsBase + 0x10;
        private const int ClassSBars = ClassSBase + 0x68;
        private const int ClassABase = StatsBase + 0xFC;
        private const int ClassABars = ClassABase + 0x48;
        private const int ClassBBase = StatsBase + 0x1AC;
        private const int ClassBBars = ClassBBase + 0x58;
        private const int ClassCBase = StatsBase + 0x27C;
        private const int ClassCBars = ClassCBase + 0x58;
        private const int ClassRBase = StatsBase + 0x34C;
        private const int ClassRBars = ClassRBase + 0xF0;

        static void Main(string[] args)
        {
            using (var file = File.Open("arcade", FileMode.Open, FileAccess.Read))
            {
                file.Position = CarCounts;
                ArcadeCar[] classS = new ArcadeCar[file.ReadUShort()];
                ArcadeCar[] classA = new ArcadeCar[file.ReadUShort()];
                ArcadeCar[] classB = new ArcadeCar[file.ReadUShort()];
                ArcadeCar[] classC = new ArcadeCar[file.ReadUShort()];
                file.Position += 4; // skip two counts of 00 04 - home garage?
                ArcadeCar[] classR = new ArcadeCar[file.ReadUShort()];
                file.Position = CarIDs;
                ReadCarIDs(file, classS);
                ReadCarIDs(file, classA);
                ReadCarIDs(file, classB);
                ReadCarIDs(file, classC);
                ReadCarIDs(file, classR);
                ReadCarStatsAndWriteCSV(file, classS, ClassSBars, "ClassS");
                ReadCarStatsAndWriteCSV(file, classA, ClassABars, "ClassA");
                ReadCarStatsAndWriteCSV(file, classB, ClassBBars, "ClassB");
                ReadCarStatsAndWriteCSV(file, classC, ClassCBars, "ClassC");
                ReadCarStatsAndWriteCSV(file, classR, ClassRBars, "ClassR");
            }
        }

        private static void ReadCarIDs(Stream file, ArcadeCar[] cars)
        {
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i] = new ArcadeCar { ID = file.ReadCharacters() };
                file.MoveToNextMultipleOf(8);
            }
        }

        private static void ReadCarStatsAndWriteCSV(Stream file, ArcadeCar[] cars, int barsPosition, string csvName)
        {
            file.Position = barsPosition;
            foreach (ArcadeCar car in cars)
            {
                car.MaxSpeed = file.ReadSingleByte();
                car.Handling = file.ReadSingleByte();
                car.Acceleration = file.ReadSingleByte();
            }
            file.MoveToNextMultipleOf(4);
            foreach (ArcadeCar car in cars)
            {
                car.Power = file.ReadUShort();
                car.PowerRPM = file.ReadUShort();
                car.Torque = file.ReadUShort();
                car.TorqueRPM = file.ReadUShort();
                car.Weight = file.ReadUShort();
            }

            DumpClassToCSV(csvName, cars);
        }

        private static void DumpClassToCSV(string name, ArcadeCar[] cars)
        {
            using (TextWriter file = new StreamWriter(File.Create($"{name}.csv"), Encoding.UTF8))
            {
                using (var csv = new CsvWriter(file, new Configuration() { QuoteAllFields = true }))
                {
                    csv.WriteField("ID");
                    csv.WriteField("Power");
                    csv.WriteField("PowerRPM");
                    csv.WriteField("Torque");
                    csv.WriteField("TorqueRPM");
                    csv.WriteField("Weight");
                    csv.WriteField("MaxSpeed");
                    csv.WriteField("Handling");
                    csv.WriteField("Acceleration");
                    csv.NextRecord();

                    foreach (ArcadeCar car in cars)
                    {
                        csv.WriteField(car.ID);
                        csv.WriteField(car.Power);
                        csv.WriteField(car.PowerRPM);
                        csv.WriteField(car.Torque);
                        csv.WriteField(car.TorqueRPM);
                        csv.WriteField(car.Weight);
                        csv.WriteField(car.MaxSpeed);
                        csv.WriteField(car.Handling);
                        csv.WriteField(car.Acceleration);
                        csv.NextRecord();
                    }
                }
            }
        }
    }
}