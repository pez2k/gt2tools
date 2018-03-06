using System.Collections.Generic;
using System.IO;

namespace GT2UsedCarEditor
{
    class UsedCarList
    {
        public List<TimePeriod> TimePeriods { get; set; } = new List<TimePeriod>(60);

        public void Read(Stream stream)
        {
            for (int i = 0; i < 60; i++)
            {
                stream.Position = (i * 4) + 8;
                var period = new TimePeriod();
                period.Read(stream, stream.ReadUInt());
                TimePeriods.Add(period);
            }
        }

        public void WriteCSV()
        {
            for (int i = 0; i < TimePeriods.Count; i++)
            {
                string directoryName = string.Format("{0:000}", i * 10);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                TimePeriods[i].WriteCSV(directoryName);
            }
        }
    }
}
