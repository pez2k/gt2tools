using System;
using System.Collections.Generic;
using System.IO;

namespace GT2.CarInfoEditor
{
    public class FileSet : IDisposable
    {
        public Stream JPCarInfo { get; set; }
        public Stream USCarInfo { get; set; }
        public Stream EUCarInfo { get; set; }
        public Stream CCLatin { get; set; }
        public Stream CCJapanese { get; set; }
        public Stream CarColours { get; set; }

        public List<Stream> CarInfoFiles
        {
            get
            {
                return new List<Stream> { JPCarInfo, USCarInfo, EUCarInfo };
            }
        }

        public static FileSet OpenRead()
        {
            FileSet fileset = new FileSet();
            fileset.JPCarInfo = new FileStream(".carinfoj", FileMode.Open, FileAccess.Read);
            fileset.USCarInfo = new FileStream(".carinfoa", FileMode.Open, FileAccess.Read);
            fileset.EUCarInfo = new FileStream(".carinfoe", FileMode.Open, FileAccess.Read);
            fileset.CCLatin = new FileStream(".cclatain", FileMode.Open, FileAccess.Read);
            fileset.CCJapanese = new FileStream(".ccjapanese", FileMode.Open, FileAccess.Read);
            fileset.CarColours = new FileStream(".carcolor", FileMode.Open, FileAccess.Read);
            return fileset;

            //JPCarInfo = new FileStream(".carinfo", FileMode.Open, FileAccess.Read);
        }

        public static FileSet OpenWrite()
        {
            FileSet fileset = new FileSet();
            fileset.JPCarInfo = new FileStream(".carinfoj", FileMode.Create, FileAccess.Write);
            fileset.USCarInfo = new FileStream(".carinfoa", FileMode.Create, FileAccess.Write);
            fileset.EUCarInfo = new FileStream(".carinfoe", FileMode.Create, FileAccess.Write);
            fileset.CCLatin = new FileStream(".cclatain", FileMode.Create, FileAccess.Write);
            fileset.CCJapanese = new FileStream(".ccjapanese", FileMode.Create, FileAccess.Write);
            fileset.CarColours = new FileStream(".carcolor", FileMode.Create, FileAccess.Write);
            return fileset;
        }

        public void Dispose()
        {
            JPCarInfo.Dispose();
            USCarInfo.Dispose();
            EUCarInfo.Dispose();
            CCLatin.Dispose();
            CCJapanese.Dispose();
            CarColours.Dispose();
        }
    }
}
