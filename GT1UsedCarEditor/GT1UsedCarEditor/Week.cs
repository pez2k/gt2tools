namespace GT1.UsedCarEditor
{
    public class Week
    {
        private static readonly string[] manufacturers = new string[] { "Toyota", "Nissan", "Mitsubishi", "Honda", "Mazda", "Subaru" };

        public Manufacturer[] Manufacturers  { get; set; } = Array.Empty<Manufacturer>();

        public static Week ReadFromFile(Stream file) =>
            new Week
            {
                Manufacturers = manufacturers.Select(name => Manufacturer.ReadFromFile(file, name)).ToArray()
            };

        public void WriteToCSV(string directory, int weekNumber)
        {
            string weekDirectory = Path.Combine(directory, $"{weekNumber * 10:000}");
            if (!Directory.Exists(weekDirectory))
            {
                Directory.CreateDirectory(weekDirectory);
            }
            foreach (Manufacturer manufacturer in Manufacturers)
            {
                manufacturer.WriteToCSV(weekDirectory);
            }
        }

        public static Week ReadFromCSV(string directory) =>
            new Week()
            {
                Manufacturers = manufacturers.Select(name => Manufacturer.ReadFromCSV(Path.Combine(directory, $"{name}.csv"), name)).ToArray()
            };

        public void WriteToFile(Stream file)
        {
            foreach (Manufacturer manufacturer in Manufacturers)
            {
                manufacturer.WriteToFile(file);
            }
        }
    }
}