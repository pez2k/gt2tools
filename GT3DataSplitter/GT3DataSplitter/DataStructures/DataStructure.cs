using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StreamExtensions;

namespace GT3.DataSplitter
{
    public class DataStructure
    {
        public string Name => GetType().Name;

        public int Size { get; set; }

        public byte[] RawData { get; set; }

        public bool IsDuplicate { get; set; }

        protected const string DuplicateTag = "___DUPLICATE";

        public virtual void Read(Stream infile)
        {
            RawData = new byte[Size];
            infile.Read(RawData, 0, Size);
        }

        public virtual void Dump()
        {
            string filename = CreateOutputFilename(RawData);

            while (File.Exists(filename))
            {
                filename += DuplicateTag;
            }

            using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.Write))
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
            if (filename.EndsWith(DuplicateTag))
            {
                IsDuplicate = true;
            }

            using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                ImportStructure(infile);
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

        public static void Import<T>(this List<T> structures) where T : DataStructure, new()
        {
            T example = new T();
            Console.WriteLine($"Importing {example.Name} structures from disk...");

            // TODO: implement read ordering correctly - Engine for example was originally read by car manufacturer ID then ordinal part ID - can be deduced from unistr entries
            foreach (string filename in Directory.EnumerateFiles(example.Name))
            {
                T structure = new T();
                structure.Import(filename);
                structures.Add(structure);
            }
        }

        public static void Write<T>(this List<T> structures, FileStream file, uint dataStart, ushort tableNumber) where T : DataStructure, new()
        {
            uint tableStart = (uint)file.Position;

            file.WriteCharacters("GTDT");
            file.WriteUShort(0x0827);
            file.WriteUShort(tableNumber);
            file.WriteUShort((ushort)structures.Count);
            file.WriteUShort((ushort)new T().Size);
            long sizePosition = file.Position;
            file.Position += 4;

            structures.Sort(new Comparison<T>((a, b) => {
                int hashComparison = a.RawData.ReadULong().CompareTo(b.RawData.ReadULong());
                if (hashComparison == 0)
                {
                    if (a.IsDuplicate)
                    {
                        return 1;
                    }
                    else if (b.IsDuplicate)
                    {
                        return -1;
                    }
                }
                return hashComparison;
            }));

            foreach (T structure in structures)
            {
                structure.Write(file);
            }

            uint tableSize = (uint)file.Position - tableStart;
            file.Position = sizePosition;
            file.WriteUInt(tableSize);

            file.Position = (tableNumber * 4) + 16;
            file.WriteUInt(tableStart - dataStart);
            file.Position = file.Length;
        }
    }
}
