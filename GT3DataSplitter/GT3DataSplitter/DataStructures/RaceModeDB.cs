using System.Collections.Generic;
using System.IO;

namespace GT3.DataSplitter
{
    public class RaceModeDB
    {
        public List<RaceMode> RaceModes { get; set; } = new List<RaceMode>();

        public void ReadData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                RaceModes.Read(file);
            }
        }

        public void DumpData()
        {
            RaceModes.Dump();
        }
    }
}