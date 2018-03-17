using System.IO;

namespace GT2.DataSplitter
{
    using CarNameConversion;
    using StreamExtensions;

    public class CarDataStructure : DataStructure
    {
        public bool HasCarId { get; set; } = true;

        public override string CreateOutputFilename(byte[] data)
        {
            string filename = Name;

            if (HasCarId)
            {
                uint carID = data.ReadUInt();
                filename += "\\" + carID.ToCarName();

                if (!Directory.Exists(filename))
                {
                    Directory.CreateDirectory(filename);
                }
            }

            string number = Directory.GetFiles(filename).Length.ToString();

            for (int i = number.Length; i < 4; i++)
            {
                number = "0" + number;
            }

            return filename + "\\" + number + "0.dat";
        }
    }
}
