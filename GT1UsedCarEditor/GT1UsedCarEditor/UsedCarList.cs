using StreamExtensions;

namespace GT1.UsedCarEditor
{
    public class UsedCarList
    {
        public Week[] Weeks { get; set; } = Array.Empty<Week>();

        public static UsedCarList ReadFromFile(Stream file)
        {
            const int WeekCount = 60;
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
    }
}