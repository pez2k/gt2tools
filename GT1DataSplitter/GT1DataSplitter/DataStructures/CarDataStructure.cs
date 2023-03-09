using System.IO;

namespace GT1.DataSplitter
{
    using StreamExtensions;

    public class CarDataStructure : DataStructure
    {
        protected bool hasCarId = true;

        protected override string CreateOutputFilename()
        {
            string filename = Name;

            if (hasCarId)
            {
                //uint carID = rawData.ReadUInt();
                //filename += "\\" + carID.ToCarName();

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