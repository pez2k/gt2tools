using System.Collections.Generic;
using System.IO;

namespace GT2DataSplitter
{
    public class DataStructure
    {
        public string Name { get; set; }
        public int Size { get; set; }

        public DataStructure(int Size)
        {
            Name = GetType().Name;
            this.Size = Size;
        }

        public virtual void ReadData(FileStream infile, uint blockStart, uint blockSize)
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

                using (FileStream outfile = new FileStream(CreateOutputFilename(data), FileMode.Create, FileAccess.Write))
                {
                    ExportStructure(data, outfile);
                }
            }
        }

        public virtual string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Directory.GetFiles(Name).Length.ToString() + "0.dat";
        }

        public virtual void ExportStructure(byte[] structure, FileStream output)
        {
            output.Write(structure, 0, structure.Length);
        }

        public virtual void WriteData(FileStream outfile, uint indexPosition)
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
                        ImportStructure(infile, outfile);
                    }
                }
            }

            uint blockSize = (uint)outfile.Position - startingPosition;
            outfile.Position = indexPosition;
            outfile.WriteUInt(startingPosition);
            outfile.WriteUInt(blockSize);
        }

        public virtual void ImportStructure(FileStream structure, FileStream output)
        {
            structure.CopyTo(output);
        }
    }
}
