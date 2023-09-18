using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StreamExtensions;

namespace GT1.DataSplitter
{
    using Caches;
    using LZSS;

    public class DataFile
    {
        public static string OverridePath { get; set; }

        private const int Windows31J = 932; // Windows-31J, code page 932

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
            if (fileType != template.Header)
            {
                throw new Exception($"Mismatched header for {template.Name}!");
            }

            file.Position = blockStart + 0x0C;

            file.ReadUShort(); // always 0x10?

            ushort structCount = file.ReadUShort();
            file.ReadUInt(); // always zero?
            uint structSize = file.ReadUInt();

            if (structSize != template.Size)
            {
                throw new Exception($"Invalid block size for {template.Name}!");
            }

            for (ushort i = 0; i < structCount; i++)
            {
                var structure = (DataStructure)Activator.CreateInstance(data.Type);
                structure.Parent = data;
                structure.Read(file);
                data.Structures.Add(structure);
            }

            if (file.Position < (blockStart + blockSize))
            {
                data.StringTables = ReadStringTables(file);
            }
        }

        private static List<List<string>> ReadStringTables(Stream file)
        {
            uint tableCount = file.ReadUInt();
            file.Position += tableCount * 4;
            List<List<string>> strings = new((int)tableCount);

            for (uint i = 0; i < tableCount; i++)
            {
                strings.Add(ReadStrings(file));
            }
            return strings;
        }

        private static List<string> ReadStrings(Stream file)
        {
            ushort stringCount = file.ReadUShort();
            List<string> strings = new(stringCount);
            for (ushort i = 0; i < stringCount; i++)
            {
                byte stringLength = file.ReadSingleByte();
                var buffer = new byte[stringLength];
                file.Read(buffer);
                string textString = Encoding.GetEncoding(Windows31J).GetString(buffer);
                strings.Add(textString);
                file.Position++;
            }
            file.MoveToNextMultipleOf(2);
            return strings;
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

            if (template.StringTableCount == 0)
            {
                DumpStringTables(data.StringTables, template.Name);
            }
        }

        private static void DumpStringTables(List<List<string>> stringTables, string directory)
        {
            for (int i = 0; i < stringTables.Count; i++)
            {
                using (StreamWriter output = new(Path.Combine(directory, $"strings{i:D2}.txt"), false, Encoding.UTF8))
                {
                    foreach (string textString in stringTables[i])
                    {
                        output.WriteLine(textString);
                    }
                    output.Flush();
                }
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
            data.StringTables = template.StringTableCount > 0 ? Enumerable.Range(0, template.StringTableCount).Select(i => new List<string>()).ToList() : ImportStringTables(template.Name);

            bool hasOverrides = OverridePath != null && Directory.Exists(Path.Combine(OverridePath, template.Name));
            foreach (string baseFilename in Directory.EnumerateFiles(template.Name, "*.dat").Concat(Directory.EnumerateFiles(template.Name, "*.csv")))
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
                        structure.Parent = data;
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

        private static List<List<string>> ImportStringTables(string directory)
        {
            string[] files = Directory.EnumerateFiles(directory, "strings*.txt").ToArray();
            List<List<string>> stringTables = new(files.Length);
            foreach (string file in files)
            {
                using (StreamReader input = new(file, Encoding.UTF8))
                {
                    List<string> strings = new();
                    while (!input.EndOfStream)
                    {
                        strings.Add(input.ReadLine());
                    }
                    stringTables.Add(strings);
                }
            }
            return stringTables;
        }

        public void WriteData(string filename, int windowSize)
        {
            using (MemoryStream stream = new())
            {
                WriteDataToFile(stream);
                stream.Position = 0;

                using (FileStream file = new(filename, FileMode.Create, FileAccess.ReadWrite))
                {
                    if (windowSize == 0)
                    {
                        LZSS.FakeCompress(stream, file);
                    }
                    else if (windowSize < 0)
                    {
                        stream.CopyTo(file);
                    }
                    else
                    {
                        LZSS.Compress(stream, file, windowSize);
                    }
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
                WriteStringTables(data.StringTables, innerFile);
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

        private static void WriteStringTables(List<List<string>> stringTables, Stream file)
        {
            int tableCount = stringTables.Count;
            file.WriteUInt((uint)tableCount);
            file.Position += tableCount * 4;

            foreach (List<string> strings in stringTables)
            {
                WriteStrings(strings, file);
            }
        }

        private static void WriteStrings(List<string> strings, Stream file)
        {
            int stringCount = strings.Count;
            file.WriteUShort((ushort)stringCount);
            foreach (string textString in strings)
            {
                byte[] textData = Encoding.GetEncoding(Windows31J).GetBytes(textString);
                int stringLength = textData.Length;
                file.WriteByte((byte)stringLength);
                file.Write(textData);
                file.WriteByte(0);
            }
            file.MoveToNextMultipleOf(2);
        }
    }
}