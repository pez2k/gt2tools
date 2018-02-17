using System.IO;

namespace GT2DataSplitter
{
    public class CarDataStructure : DataStructure
    {
        public bool HasCarId { get; set; }

        public CarDataStructure(int Size, bool HasCarId = true) : base(Size)
        {
            this.HasCarId = HasCarId;
        }

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
