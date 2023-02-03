using StreamExtensions;

namespace GT1.UsedCarEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] manufacturers = new string[] { "Toyota", "Nissan", "Mitsubishi", "Honda", "Mazda", "Subaru" };

            using (FileStream file = new("_unknown0004.usedcar", FileMode.Open, FileAccess.Read))
            {
                using (TextWriter output = new StreamWriter("dump.txt"))
                {
                    file.Position = 0x10; // skip header
                    const int WeekCount = 60;

                    uint[] blockPointers = new uint[WeekCount];
                    for (int i = 0; i < WeekCount; i++)
                    {
                        blockPointers[i] = file.ReadUInt();
                    }

                    for (int i = 0; i < WeekCount; i++)
                    {
                        file.Position = blockPointers[i];
                        output.WriteLine($"Week {i}");
                        output.WriteLine();

                        foreach (string manufacturer in manufacturers)
                        {
                            output.WriteLine(manufacturer);
                            uint carCount = file.ReadUInt();
                            output.WriteLine($"{carCount} cars");
                            output.WriteLine();

                            for (int j = 0; j < carCount; j++)
                            {
                                ushort price = file.ReadUShort();
                                byte carID = file.ReadSingleByte();
                                byte colourID = file.ReadSingleByte();
                                output.WriteLine($"Car ID: {carID:X2} Colour ID: {colourID:X2} Price: {price * 10} Cr");
                            }
                            output.WriteLine();
                        }
                        output.WriteLine();
                        output.WriteLine();
                    }
                }
            }
        }
    }
}