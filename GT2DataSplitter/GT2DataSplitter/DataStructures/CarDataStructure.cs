using System.IO;

namespace GT2DataSplitter
{
    public class CarDataStructure : DataStructure
    {
        public bool HasCarId { get; set; } = true;

        public override string CreateOutputFilename(byte[] data)
        {
            string filename = Name;

            if (HasCarId)
            {
                uint carID = data.ReadUInt();
                filename += "\\" + Utils.GetCarName(carID);

                if (!Directory.Exists(filename))
                {
                    Directory.CreateDirectory(filename);
                }
            }

            return filename + "\\" + Directory.GetFiles(filename).Length.ToString() + "0.dat";
        }
    }
}
