﻿using System.IO;

namespace GT1.DataSplitter
{
    using Caches;

    public abstract class DataStructure
    {
        public string Name => GetType().Name;
        public string Header { get; protected set; }
        public int Size { get; protected set; }
        public TypedData Parent { get; set; }

        protected string filenameCacheNameOverride;
        protected byte[] rawData;

        public virtual void Read(Stream infile)
        {
            rawData = new byte[Size];
            infile.Read(rawData, 0, Size);
        }

        public virtual void Dump()
        {
            string filename = CreateOutputFilename();
            using (FileStream outfile = new(filename, FileMode.Create, FileAccess.Write))
            {
                ExportStructure(rawData, outfile);
                FileNameCache.Add(filenameCacheNameOverride ?? Name, filename);
            }
        }

        protected virtual string CreateOutputFilename() => CreateOutputFilenameBase();

        protected string CreateDetailedOutputFilename(int carIDOffset)
        {
            string filename = CreateOutputFilenameBase();
            int priceOffset = carIDOffset + 3;
            while (priceOffset % 4 != 0)
            {
                priceOffset++;
            }
            return filename.Replace(Path.GetExtension(filename),
                $"_car{rawData[carIDOffset]:X2}" +
                $"_stage{(sbyte)rawData[carIDOffset + 2] + 1:X2}" +
                $"_{Parent.StringTables[0][rawData[priceOffset + 4]]}" +
                $" {Parent.StringTables[1][rawData[priceOffset + 8]].Replace('/', '-')}" +
                Path.GetExtension(filename));
        }

        private string CreateOutputFilenameBase() => Path.Combine(Name, $"{Directory.GetFiles(Name).Length + 1:D4}.dat");

        private static void ExportStructure(byte[] structure, FileStream output) => output.Write(structure, 0, structure.Length);

        public virtual void Import(string filename)
        {
            ImportStructure(FileContentsCache.GetFile(filename));
            FileNameCache.Add(filenameCacheNameOverride ?? Name, filename);
        }

        private void ImportStructure(byte[] file) => rawData = file;

        public virtual void Write(Stream outfile) => outfile.Write(rawData, 0, Size);
    }
}