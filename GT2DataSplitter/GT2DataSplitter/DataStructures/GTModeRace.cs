using System.IO;

namespace GT2.DataSplitter
{
    using StreamExtensions;

    public class GTModeRace : DataFile
    {
        public GTModeRace() : base((typeof(Event), 2),
                                   (typeof(EnemyCars), 1),
                                   (typeof(Regulations), 0))
        {
        }

        protected override void ReadDataFromFile(Stream file)
        {
            base.ReadDataFromFile(file);
            uint blockStart = file.ReadUInt();
            uint blockSize = file.ReadUInt(); // unused
            ASCIIStringTable.Read(file, blockStart);
        }

        protected override void WriteDataToFile(Stream file)
        {
            base.WriteDataToFile(file);
            ASCIIStringTable.Write(file, file.Position);
        }
    }
}