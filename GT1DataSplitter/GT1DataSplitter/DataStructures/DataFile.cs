using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using StreamExtensions;

namespace GT1.DataSplitter
{
    using Caches;
    using LZSS;

    public class DataFile
    {
        public static string OverridePath { get; set; }

        private readonly TypedData[] data;

        public DataFile(params (Type type, int orderOnDisk, bool isLocalised)[] dataDefinitions) =>
            data = dataDefinitions.Select(definition => new TypedData(definition.type, definition.orderOnDisk, definition.isLocalised)).ToArray();

        public void ReadData(string filename)
        {
            using (FileStream file = new(filename, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream decompressed = new())
                {
                    LZSS.Decompress(file, decompressed);
                    ReadDataFromFile(decompressed);
                }
            }
        }

        protected virtual void ReadDataFromFile(Stream stream)
        {
            stream.Position = 0x0E;
            ushort fileCount = stream.ReadUShort();
            if (fileCount != data.Length)
            {
                throw new Exception("Unexpected file count");
            }

            for (ushort i = 0; i < data.Length; i++)
            {
                uint offset = stream.ReadUInt();
                uint size = stream.ReadUInt();
                uint uncompressedSize = stream.ReadUInt();

                if (size != uncompressedSize)
                {
                    throw new Exception("Compressed file found");
                }
                else
                {
                    long indexPosition = stream.Position;
                    Read(stream, offset, size, data[i]);
                    stream.Position = indexPosition;
                }
            }
        }

        private static void Read(Stream file, uint blockStart, uint blockSize, TypedData data)
        {
            var template = (DataStructure)Activator.CreateInstance(data.Type);
            Console.WriteLine($"Reading {template.Name} structures from file...");

            file.Position = blockStart + 4; // skip @(#) header start
            string fileType = file.ReadCharacters();

            file.Position = blockStart + 0x0C;

            file.ReadUShort(); // always 0x10?

            ushort structCount = file.ReadUShort();
            file.ReadUInt(); // always zero?
            uint structSize = file.ReadUInt();

            if (structSize != template.Size)
            {
                Console.WriteLine($"Invalid block size for {template.Name}!");
                return;
            }

            for (ushort i = 0; i < structCount; i++)
            {
                /*var buffer = new byte[structSize];
                file.Read(buffer);
                structs.Add(buffer);*/
                var structure = (DataStructure)Activator.CreateInstance(data.Type);
                structure.Read(file);
                data.Structures.Add(structure);
            }

            /*List<List<string>> stringTables = null;
            if (file.Position < file.Length)
            {
                stringTables = ReadStringTables(file);
            }*/


            /*long previousPosition = file.Position;
            file.Position = blockStart;
            long blockCount = blockSize / template.Size;*/

            //file.Position = previousPosition;
        }

        public void DumpData()
        {
            foreach (TypedData item in data.OrderBy(item => item.OrderOnDisk))
            {
                Dump(item);
            }
        }

        private static void Dump(TypedData data)
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

        /*public void ImportData()
        {
            foreach (TypedData item in data.OrderBy(item => item.OrderOnDisk))
            {
                Import(item);
            }
        }

        private static void Import(TypedData data)
        {
            var template = (DataStructure)Activator.CreateInstance(data.Type);
            Console.WriteLine($"Importing {template.Name} structures from disk...");

            Dictionary<uint, string> cars = new();
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
                    if (!FileExistsCache.FileExists(filename))
                    {
                        filename = baseFilename;
                    }

                    if (!FileIsEmptyCache.FileIsEmpty(filename))
                    {
                        DataStructure structure = DataStructureCache.GetStructure(filename);
                        if (structure == null)
                        {
                            structure = (DataStructure)Activator.CreateInstance(data.Type);
                            structure.Import(filename);
                            if (!data.IsLocalised)
                            {
                                DataStructureCache.Add(filename, structure);
                            }
                        }
                        data.Structures.Add(structure);
                    }
                }
            }
        }

        public void WriteData(string filename)
        {
            using (FileStream file = new(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                WriteDataToFile(file);

                if (file.Length > 0xC8000)
                {
                    throw new Exception($"{filename} exceeds 800kb size limit.");
                }

                file.Position = 0;
                using (FileStream zipFile = new(filename + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream zip = new(zipFile, CompressionMode.Compress))
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

        private static void Write(TypedData data, Stream file, int indexPosition)
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
        }*/
    }
}