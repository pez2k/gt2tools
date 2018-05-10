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
            using (FileSet files = new FileSet())
            {
                files.JPCarInfo.Position = 0x04; // Skip header
                uint carCount = files.JPCarInfo.ReadUInt();

                Cars = new List<Car>();

                for (uint i = 0; i < carCount; i++)
                {
                    Car car = new Car();
                    car.ReadFromFiles(files, i);
                    Cars.Add(car);
                }
            }
        }
    }
}
