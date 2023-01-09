using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace GT2.DataSplitter
{
    using Caches;

    public abstract class CsvDataStructure<TStructure, TMap> : DataStructure where TMap : ClassMap
    {
        protected bool cacheFilename;
        protected TStructure data;

        protected CsvDataStructure() => Size = Marshal.SizeOf<TStructure>();

        protected override string CreateOutputFilename() => base.CreateOutputFilename().Replace(".dat", ".csv");

        public override void Read(Stream infile)
        {
            base.Read(infile);

            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            data = (TStructure)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TStructure));
            handle.Free();
        }

        public override void Dump()
        {
            string filename = CreateOutputFilename();
            using (TextWriter output = new StreamWriter(File.Create(filename), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(output))
                {
                    csv.Configuration.RegisterClassMap<TMap>();
                    csv.Configuration.QuoteAllFields = true;
                    csv.WriteHeader<TStructure>();
                    csv.NextRecord();
                    csv.WriteRecord(data);
                    if (cacheFilename)
                    {
                        FileNameCache.Add(filenameCacheNameOverride ?? Name, filename);
                    }
                }
            }
        }

        public override void Import(string filename)
        {
            try
            {
                using (var stream = new MemoryStream(FileContentsCache.GetFile(filename)))
                {
                    using (TextReader input = new StreamReader(stream, Encoding.UTF8))
                    {
                        using (CsvReader csv = new CsvReader(input))
                        {
                            csv.Configuration.RegisterClassMap<TMap>();
                            csv.Read();
                            data = csv.GetRecord<TStructure>();
                            if (cacheFilename)
                            {
                                FileNameCache.Add(filenameCacheNameOverride ?? Name, filename);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"Could not import CSV: {filename}", exception);
            }
        }

        public override void Write(Stream outfile)
        {
            int size = Marshal.SizeOf(data);
            rawData = new byte[size];

            IntPtr objectPointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, objectPointer, true);
            Marshal.Copy(objectPointer, rawData, 0, size);
            Marshal.FreeHGlobal(objectPointer);

            base.Write(outfile);
        }
    }
}