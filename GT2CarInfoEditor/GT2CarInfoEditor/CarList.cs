using System.Collections.Generic;
using System.IO;

namespace GT2.CarInfoEditor
{
    using StreamExtensions;

    public class CarList
    {
        public List<Car> Cars { get; set; }

        public void ReadFromFiles()
        {
            using (FileSet files = FileSet.OpenRead())
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

                CarColour.ClearCache();
            }
        }

        public void SaveToFiles()
        {
            using (FileSet files = FileSet.OpenWrite())
            {
                byte[] header = "CAR\0".ToByteArray();
                foreach (Stream file in files.CarInfoFiles)
                {
                    file.Write(header);
                    file.WriteUInt((uint)Cars.Count);
                    file.SetLength((Cars.Count + 1) * 8);
                }
                
                for (int i = 0; i < Cars.Count; i++)
                {
                    Cars[i].WriteToFiles(files, (uint)i);
                }
            }
        }
    }
}
