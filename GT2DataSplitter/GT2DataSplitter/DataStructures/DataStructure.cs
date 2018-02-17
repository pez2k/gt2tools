using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT2DataSplitter
{
    public class DataStructure
    {
        public string Name { get; set; }
        public int Size { get; set; }

        public DataStructure(int Size)
        {
            this.Name = GetType().Name;
            this.Size = Size;
        }

        public void ReadData(FileStream infile, uint blockStart, uint blockSize)
        {
            if (blockSize % Size > 0)
            {
                return;
            }

            if (!Directory.Exists(Name))
            {
                Directory.CreateDirectory(Name);
            }

            infile.Position = blockStart;
            long blockCount = blockSize / Size;

            for (int i = 0; i < blockCount; i++)
            {
                byte[] data = new byte[Size];
                infile.Read(data, 0, Size);

                string filename = Name;

                if (blockCount > 1)
                {
                    uint carID = data.ReadUInt();
                    filename += "\\" + Utils.GetCarName(carID);

                    if (!Directory.Exists(filename))
                    {
                        Directory.CreateDirectory(filename);
                    }
                }

                filename += "\\" + Directory.GetFiles(filename).Length.ToString() + "0.dat";

                using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    outfile.Write(data, 0, data.Length);
                }
            }
        }

        public void WriteData(FileStream outfile, uint indexPosition)
        {
            Dictionary<uint, string> cars = new Dictionary<uint, string>();

            foreach (string carName in Directory.EnumerateDirectories(Name))
            {
                cars.Add(Utils.GetCarID(carName), carName);
            }

            if (cars.Count == 0)
            {
                cars.Add(0, Name);
            }

            outfile.Position = outfile.Length;
            uint startingPosition = (uint)outfile.Position;

            foreach (string carName in cars.Values)
            {
                foreach (string filename in Directory.EnumerateFiles(carName))
                {
                    using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        infile.CopyTo(outfile);
                    }
                }
            }

            uint blockSize = (uint)outfile.Position - startingPosition;
            outfile.Position = indexPosition;
            outfile.WriteUInt(startingPosition);
            outfile.WriteUInt(blockSize);
        }
    }
}
