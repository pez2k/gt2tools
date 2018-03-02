using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GT2DataSplitter
{
    public class CsvDataStructure : DataStructure
    {
        public bool CacheFilename { get; set; } = false;

        public override string CreateOutputFilename(byte[] data)
        {
            return base.CreateOutputFilename(data).Replace(".dat", ".csv");
        }

        public TStructure ReadStructure<TStructure>(FileStream infile) where TStructure : StructureData
        {
            base.Read(infile);

            GCHandle handle = GCHandle.Alloc(RawData, GCHandleType.Pinned);
            TStructure data = (TStructure)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TStructure));
            handle.Free();
            return data;
        }

        public void DumpCsv<TStructure, TMap>(TStructure data) where TStructure : StructureData where TMap : ClassMap
        {
            string filename = CreateOutputFilename(RawData);
            using (TextWriter output = new StreamWriter(File.Create(filename), Encoding.UTF8))
            {
                using (CsvWriter csv = new CsvWriter(output))
                {
                    csv.Configuration.RegisterClassMap<TMap>();
                    csv.Configuration.QuoteAllFields = true;
                    csv.WriteHeader<TStructure>();
                    csv.NextRecord();
                    csv.WriteRecord(data);
                    if (CacheFilename)
                    {
                        FileNameCache.Add(Name, filename);
                    }
                }
            }
        }

        public TStructure ImportCsv<TStructure, TMap>(string filename) where TStructure : StructureData where TMap : ClassMap
        {
            using (TextReader input = new StreamReader(filename, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(input))
                {
                    csv.Configuration.RegisterClassMap<TMap>();
                    csv.Read();
                    TStructure data = csv.GetRecord<TStructure>();
                    if (CacheFilename)
                    {
                        FileNameCache.Add(Name, filename);
                    }
                    return data;
                }
            }
        }

        public void WriteStructure<TStructure>(FileStream outfile, TStructure data) where TStructure : StructureData
        {
            int size = Marshal.SizeOf(data);
            RawData = new byte[size];

            IntPtr objectPointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, objectPointer, true);
            Marshal.Copy(objectPointer, RawData, 0, size);
            Marshal.FreeHGlobal(objectPointer);

            base.Write(outfile);
        }
        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class StructureData
        {
        }
    }
}
