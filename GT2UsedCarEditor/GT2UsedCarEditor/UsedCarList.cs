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

        public void WriteCSV(string baseDirectory)
        {
            for (int i = 0; i < TimePeriods.Count; i++)
            {
                string directoryName = baseDirectory + "\\" + string.Format("{0:000}", i * 10);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                TimePeriods[i].WriteCSV(directoryName);
            }
        }

        public void ReadCSV(string baseDirectory)
        {
            foreach (string directory in Directory.GetDirectories(baseDirectory))
            {
                var period = new TimePeriod();
                period.ReadCSV(directory);
                TimePeriods.Add(period);
            }
        }

        public void Write(Stream stream)
        {
            stream.Position = 0;
            stream.Write(new byte[] { 0x55, 0x43, 0x41, 0x52, 0x00, 0x00, 0x00, 0x00 }, 0, 8); // UCAR header
            uint dataPosition = (uint)((TimePeriods.Count + 1) * 4) + 8;

            for (int i = 0; i < TimePeriods.Count; i++)
            {
                dataPosition = TimePeriods[i].Write(stream, (i * 4) + 8, dataPosition);
            }
            
            stream.Position = (TimePeriods.Count * 4) + 8;
            stream.WriteUInt((uint)stream.Length);
        }
    }
}
