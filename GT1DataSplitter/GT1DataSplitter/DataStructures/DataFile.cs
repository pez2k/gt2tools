using System;
using System.IO;
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
                var structure = (DataStructure)Activator.CreateInstance(data.Type);
                structure.Read(file);
                data.Structures.Add(structure);
            }

            /*List<List<string>> stringTables = null;
            if (file.Position < file.Length)
            {
                stringTables = ReadStringTables(file);
            }*/
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

        public void ImportData()
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

            bool hasOverrides = OverridePath != null && Directory.Exists(Path.Combine(OverridePath, template.Name));
            foreach (string baseFilename in Directory.EnumerateFiles(template.Name))
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

        public void WriteData(string filename)
        {
            using (MemoryStream stream = new())
            {
                WriteDataToFile(stream);
                stream.Position = 0;

                using (FileStream file = new(filename, FileMode.Create, FileAccess.ReadWrite))
                {
                    LZSS.Compress(stream, file);
                }
            }
        }

        protected virtual void WriteDataToFile(Stream file)
        {
            file.WriteCharacters("@(#)GT-ARC");
            file.WriteUShort(0);
            file.WriteUShort(1);
            file.WriteUShort((ushort)data.Length);
            file.SetLength(file.Position + (data.Length * 3 * 4));

            for (int i = 0; i < data.Length; i++)
            {
                Write(data[i], file);
            }
        }

        private static void Write(TypedData data, Stream file)
        {
            long offset = file.Length;
            long indexPosition = file.Position;
            file.Position = offset;
            uint size;

            using (MemoryStream innerFile = new())
            {
                var template = (DataStructure)Activator.CreateInstance(data.Type);
                innerFile.WriteCharacters("@(#)");
                innerFile.WriteCharacters(template.Header);
                innerFile.Position = 0x0C;
                innerFile.WriteUShort(0x10); // always 0x10?
                innerFile.WriteUShort((ushort)data.Structures.Count);
                innerFile.WriteUInt(0); // always zero?
                innerFile.WriteUInt((uint)template.Size);

                foreach (DataStructure structure in data.Structures)
                {
                    structure.Write(innerFile);
                }
                innerFile.MoveToNextMultipleOf(4);
                innerFile.SetLength(innerFile.Position);

                size = (uint)innerFile.Length;
                innerFile.Position = 0;
                innerFile.CopyTo(file);
            }

            file.Position = indexPosition;
            file.WriteUInt((uint)offset);
            file.WriteUInt(size); // not compressed, so compressed size = size
            file.WriteUInt(size);
        }
    }
}