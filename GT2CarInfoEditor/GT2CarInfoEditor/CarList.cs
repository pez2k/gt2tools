using System.Collections.Generic;
using System.IO;

namespace GT2.CarInfoEditor
{
    using StreamExtensions;

    class CarList
    {
        public List<Car> Cars { get; set; }

        public void ReadFromFiles()
        {
            using (FileStream stream = new FileStream(".carinfoe", FileMode.Open, FileAccess.Read))
            {
                stream.Position = 0x04; // Skip header
                uint carCount = stream.ReadUInt();

                Cars = new List<Car>();

                for (uint i = 0; i < carCount; i++)
                {
                    Cars.Add(Car.ReadFromFile(stream));
                }
            }
        }
    }
}
