using System.IO.Compression;

namespace GT2.DataSplitter.GTDT
{
    using StreamExtensions;

    public class DataFile<TModel> where TModel : notnull
    {
        protected readonly TypedData[] data;

        public DataFile(params Type[] dataTypes) => data = dataTypes.Select(type => new TypedData(type)).ToArray();

        public void Read(string filename)
        {
            using (Stream file = UnicodeStringTable.DecompressFile(filename))
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
                ReadBlock(file, blockStart, blockSize, data[i]);
            }
        }

        private static void ReadBlock(Stream file, uint blockStart, uint blockSize, TypedData data)
        {
            var template = (DataStructure?)Activator.CreateInstance(data.Type) ?? throw new Exception();
            Console.WriteLine($"Reading {data.Type.Name} structures from file...");

            if (blockSize % template.Size > 0)
            {
                Console.WriteLine($"Invalid block size for {data.Type.Name}!");
                return;
            }

            long previousPosition = file.Position;
            file.Position = blockStart;
            long blockCount = blockSize / template.Size;

            for (int i = 0; i < blockCount; i++)
            {
                var structure = (DataStructure?)Activator.CreateInstance(data.Type) ?? throw new Exception();
                structure.Read(file);
                data.Structures.Add(structure);
            }
            file.Position = previousPosition;
        }

        public void Write(string filename)
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
                WriteBlock(data[i], file, (i + 1) * 8);
            }
        }

        private static void WriteBlock(TypedData data, Stream file, int indexPosition)
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

        public virtual void MapToModel(TModel model, UnicodeStringTable strings) => Mapper.MapToModel(data, model, strings, new ASCIIStringTable());
    }
}