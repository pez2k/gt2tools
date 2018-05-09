using System;
using System.IO;

namespace GT2.CarInfoEditor
{
    class FileSet : IDisposable
    {
        public Stream JPCarInfo { get; set; }
        public Stream USCarInfo { get; set; }
        public Stream EUCarInfo { get; set; }
        public Stream CCLatin { get; set; }
        public Stream CCJapanese { get; set; }
        public Stream CarColours { get; set; }

        public FileSet()
        {
            JPCarInfo = new FileStream(".carinfoj", FileMode.Open, FileAccess.Read);
            USCarInfo = new FileStream(".carinfoa", FileMode.Open, FileAccess.Read);
            EUCarInfo = new FileStream(".carinfoe", FileMode.Open, FileAccess.Read);
            CCLatin = new FileStream(".cclatain", FileMode.Open, FileAccess.Read);
            CCJapanese = new FileStream(".ccjapanese", FileMode.Open, FileAccess.Read);
            CarColours = new FileStream(".carcolor", FileMode.Open, FileAccess.Read);

            //JPCarInfo = new FileStream(".carinfo", FileMode.Open, FileAccess.Read);
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
