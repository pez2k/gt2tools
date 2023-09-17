using StreamExtensions;

namespace GT1.UsedCarEditor
{
    public class UsedCarList
    {
        private const int WeekCount = 60;

        public Week[] Weeks { get; set; } = Array.Empty<Week>();

        public static UsedCarList ReadFromFile(Stream file)
        {
            file.Position = 0x10; // skip header

            uint[] blockPointers = Enumerable.Range(0, WeekCount).Select(i => file.ReadUInt()).ToArray();
            return new UsedCarList
            {
                Weeks = blockPointers.Select(pointer => ReadWeekFromFile(file, pointer)).ToArray()
            };
        }

        private static Week ReadWeekFromFile(Stream file, uint pointer)
        {
            file.Position = pointer;
            return Week.ReadFromFile(file);
        }

        public void WriteToCSV(string directory)
        {
            int weekNumber = 0;
            foreach (Week week in Weeks)
            {
                week.WriteToCSV(directory, weekNumber++);
            }
        }

        public static UsedCarList ReadFromCSV(string directory) =>
            new UsedCarList
            {
                Weeks = Directory.GetDirectories(directory).Select(directory => Week.ReadFromCSV(directory)).ToArray()
            };

        public void WriteToFile(Stream file)
        {
            file.Position = 0x10;
            long indexPosition = file.Position;
            long dataPosition = (WeekCount * 4) + 0x10;

            foreach (Week week in Weeks)
            {
                file.Position = indexPosition;
                file.WriteUInt((uint)dataPosition);
                indexPosition = file.Position;
                file.Position = dataPosition;
                week.WriteToFile(file);
                dataPosition = file.Position;
            }
        }
    }
}