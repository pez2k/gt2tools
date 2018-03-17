using System;
using System.Collections.Generic;
using System.IO;

namespace GT2.DataSplitter
{
    using CarNameConversion;
    using StreamExtensions;

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
            string filename = CreateOutputFilename(RawData);
            using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                ExportStructure(RawData, outfile);
                FileNameCache.Add(Name, filename);
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
            string number = Directory.GetFiles(Name).Length.ToString();

            for (int i = number.Length; i < 4; i++)
            {
                number = "0" + number;
            }

            return Name + "\\" + number + "0.dat";
        }

        public virtual void ExportStructure(byte[] structure, FileStream output)
        {
            output.Write(structure, 0, structure.Length);
        }

        public virtual void Import(string filename)
        {
            using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                ImportStructure(infile);
                FileNameCache.Add(Name, filename);
            }
        }

        public virtual void ImportStructure(FileStream file)
        {
            RawData = new byte[file.Length];
            file.Read(RawData, 0, (int)file.Length);
        }

        public virtual void Write(FileStream outfile)
        {
            outfile.Write(RawData, 0, Size);
        }
    }

    public static class DataStructureExtensions
    {
        public static void Read<T>(this List<T> structureList, FileStream infile, uint blockStart, uint blockSize) where T : DataStructure, new()
        {
            T structure = new T();
            Console.WriteLine($"Reading {structure.Name} structures from file...");

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
            T example = new T();
            Console.WriteLine($"Dumping {example.Name} structures to disk...");

            if (!Directory.Exists(example.Name))
            {
                Directory.CreateDirectory(example.Name);
            }

            foreach (var structure in structures)
            {
                structure.Dump();
            }
        }

        public static void Import<T>(this List<T> structures) where T : DataStructure, new()
        {
            T example = new T();
            Console.WriteLine($"Importing {example.Name} structures from disk...");

            Dictionary<uint, string> cars = new Dictionary<uint, string>();

            foreach (string carName in Directory.EnumerateDirectories(example.Name))
            {
                cars.Add(carName.ToCarID(), carName);
            }

            if (cars.Count == 0)
            {
                cars.Add(0, example.Name);
            }
            
            foreach (string carName in cars.Values)
            {
                foreach (string filename in Directory.EnumerateFiles(carName))
                {
                    T structure = new T();
                    structure.Import(filename);
                    structures.Add(structure);
                }
            }
        }

        public static void Write<T>(this List<T> structures, FileStream file, uint indexPosition) where T : DataStructure, new()
        {
            file.Position = file.Length;
            uint startingPosition = (uint)file.Position;

            foreach (T structure in structures)
            {
                structure.Write(file);
            }

            uint blockSize = (uint)file.Position - startingPosition;
            file.Position = indexPosition;
            file.WriteUInt(startingPosition);
            file.WriteUInt(blockSize);
        }
    }
}
