using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace GT2.DataSplitter
{
    using CarNameConversion;
    using StreamExtensions;

    public class DataFile
    {
        public static string OverridePath { get; set; }

        private readonly TypedData[] data;

        public DataFile(params (Type type, int orderOnDisk)[] dataDefinitions) =>
            data = dataDefinitions.Select(definition => new TypedData(definition.type, definition.orderOnDisk)).ToArray();

        public void ReadData(string filename)
        {
            using (Stream file = StringTable.DecompressFile(filename))
            {
                ReadDataFromFile(file);
            }
        }

        protected virtual void ReadDataFromFile(Stream file)
        {
            for (int i = 0; i < data.Length; i++)
            {
                file.Position = 8 * (i + 1);
                uint blockStart = file.ReadUInt();
                uint blockSize = file.ReadUInt();
                Read(file, blockStart, blockSize, data[i]);
            }
        }

        private void Read(Stream file, uint blockStart, uint blockSize, TypedData data)
        {
            var template = (DataStructure)Activator.CreateInstance(data.Type);
            Console.WriteLine($"Reading {template.Name} structures from file...");

            if (blockSize % template.Size > 0)
            {
                Console.WriteLine($"Invalid block size for {template.Name}!");
                return;
            }

            long previousPosition = file.Position;
            file.Position = blockStart;
            long blockCount = blockSize / template.Size;

            for (int i = 0; i < blockCount; i++)
            {
                var structure = (DataStructure)Activator.CreateInstance(data.Type);
                structure.Read(file);
                data.Structures.Add(structure);
            }
            file.Position = previousPosition;
        }

        public void DumpData()
        {
            foreach (TypedData item in data.OrderBy(item => item.OrderOnDisk))
            {
                Dump(item);
            }
        }

        private void Dump(TypedData data)
        {
            var template = (DataStructure)Activator.CreateInstance(data.Type);
            Console.WriteLine($"Dumping {template.Name} structures to disk...");

            if (!Directory.Exists(template.Name))
            {
                Directory.CreateDirectory(template.Name);
            }

            foreach (DataStructure structure in data.Structures)
            {
                structure.Dump();
            }
        }

        public void ImportData()
        {
            foreach (TypedData item in data.OrderBy(item => item.OrderOnDisk))
            {
                Import(item);
            }
        }

        private void Import(TypedData data)
        {
            var template = (DataStructure)Activator.CreateInstance(data.Type);
            Console.WriteLine($"Importing {template.Name} structures from disk...");

            var cars = new Dictionary<uint, string>();
            foreach (string carName in Directory.EnumerateDirectories(template.Name))
            {
                cars.Add(carName.ToCarID(), carName);
            }

            if (cars.Count == 0)
            {
                cars.Add(0, template.Name);
            }

            bool hasOverrides = OverridePath != null && Directory.Exists(Path.Combine(OverridePath, template.Name));
            foreach (string carName in cars.Values)
            {
                foreach (string baseFilename in Directory.EnumerateFiles(carName))
                {
                    string filename = hasOverrides ? Path.Combine(OverridePath, baseFilename) : baseFilename;
                    if (!File.Exists(filename))
                    {
                        filename = baseFilename;
                    }

                    if (new FileInfo(filename).Length > 0)
                    {
                        var structure = (DataStructure)Activator.CreateInstance(data.Type);
                        structure.Import(filename);
                        data.Structures.Add(structure);
                    }
                }
            }
        }

        public void WriteData(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                WriteDataToFile(file);

                if (file.Length > 0xC8000)
                {
                    throw new Exception($"{filename} exceeds 800kb size limit.");
                }

                file.Position = 0;
                using (FileStream zipFile = new FileStream(filename + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new GZipStream(zipFile, CompressionMode.Compress))
                    {
                        file.CopyTo(zip);
                    }
                }
            }
        }

        protected virtual void WriteDataToFile(Stream file)
        {
            file.WriteCharacters("GTDTl\0");
            ushort indexCount = (ushort)(data.Length * 2);
            file.WriteUShort(indexCount);
            file.Position = (indexCount * 8) + 7;
            file.WriteByte(0x00); // Data starts at end of indices, so position EOF

            for (int i = 0; i < data.Length; i++)
            {
                Write(data[i], file, (i + 1) * 8);
            }
        }

        private void Write(TypedData data, Stream file, int indexPosition)
        {
            file.Position = file.Length;
            uint startingPosition = (uint)file.Position;

            foreach (DataStructure structure in data.Structures)
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