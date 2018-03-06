using System.Collections.Generic;
using System.IO;

namespace GT2UsedCarEditor
{
    class Manufacturer
    {
        public string Name { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
        
        public void Read(Stream stream, uint startPosition, ushort carCount)
        {
            for (int i = 1; i < carCount; i++)
            {
                stream.Position = (i * 8) + startPosition;
                var car = new Car();
                car.Read(stream);
                Cars.Add(car);
            }
        }
    }
}
