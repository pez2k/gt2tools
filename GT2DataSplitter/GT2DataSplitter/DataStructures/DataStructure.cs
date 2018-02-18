using System.Collections.Generic;
using System.IO;

namespace GT2DataSplitter
{
    public class DataStructure
    {
        public string Name {
            get
            {
                return GetType().Name;
            }
        }

        public int Size { get; set; }

        public byte[] RawData { get; set; }

        public virtual void Read(FileStream infile)
        {
            RawData = new byte[Size];
            infile.Read(RawData, 0, Size);
        }

        public virtual void Dump()
        {
            CreateDirectory();

            using (FileStream outfile = new FileStream(CreateOutputFilename(RawData), FileMode.Create, FileAccess.Write))
            {
                ExportStructure(RawData, outfile);
            }
        }

        public virtual void CreateDirectory()
        {
            if (!Directory.Exists(Name))
            {
                Directory.CreateDirectory(Name);
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

    public static class DataStructureExtensions
    {
        public static void Read<T>(this List<T> structureList, FileStream infile, uint blockStart, uint blockSize) where T : DataStructure, new()
        {
            T structure = new T();

            if (blockSize % structure.Size > 0)
            {
                return;
            }

            infile.Position = blockStart;
            long blockCount = blockSize / structure.Size;

            for (int i = 0; i < blockCount; i++)
            {
                T newStructure = new T();
                newStructure.Read(infile);
                structureList.Add(newStructure);
            }
        }

        public static void Dump<T>(this List<T> structures) where T : DataStructure, new()
        {
            foreach (var structure in structures)
            {
                structure.Dump();
            }
        }
    }
}
