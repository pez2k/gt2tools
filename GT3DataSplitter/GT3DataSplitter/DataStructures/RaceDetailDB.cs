using System.Collections.Generic;
using System.IO;

namespace GT3.DataSplitter
{
    public class RaceDetailDB
    {
        public List<RaceDetail> RaceDetails { get; set; } = new List<RaceDetail>();

        public void ReadData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                RaceDetails.Read(file);
            }
        }

        public void DumpData()
        {
            RaceDetails.Dump();
        }
    }
}