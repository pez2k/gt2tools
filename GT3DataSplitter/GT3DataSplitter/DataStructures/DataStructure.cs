using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GT3.DataSplitter
{
    using StreamExtensions;

    public class DataStructure
    {
        public string Name => GetType().Name;

        public int Size { get; set; }

        public byte[] RawData { get; set; }

        public virtual void Read(Stream infile)
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
        public static void Read<TStructure>(this List<TStructure> structureList, Stream file) where TStructure : DataStructure, new()
        {
            TStructure structure = new TStructure();
            Console.WriteLine($"Reading {structure.Name} structures from file...");

            byte[] magic = new byte[4];
            file.Read(magic);
            if (Encoding.ASCII.GetString(magic) != "GTDT")
            {
                Console.WriteLine("Not a GTDT table.");
                return;
            }

            uint unknown = file.ReadUInt();
            uint structCount = file.ReadUShort();
            uint structSize = file.ReadUShort();
            if (structSize != structure.Size)
            {
                Console.WriteLine("Unexpected structure size.");
                return;
            }

            uint tableSize = file.ReadUInt();
            if (file.Length != tableSize)
            {
                Console.WriteLine("Unexpected table size.");
                return;
            }

            for (int i = 0; i < structCount; i++)
            {
                TStructure newStructure = new TStructure();
                newStructure.Read(file);
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
    }
}
