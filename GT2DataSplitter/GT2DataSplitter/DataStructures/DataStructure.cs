using System.IO;

namespace GT2.DataSplitter
{
    public abstract class DataStructure
    {
        public string Name => GetType().Name;
        public int Size { get; protected set; }

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
            using (FileStream outfile = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                ExportStructure(rawData, outfile);
                FileNameCache.Add(filenameCacheNameOverride ?? Name, filename);
            }
        }

        protected virtual string CreateOutputFilename()
        {
            string number = Directory.GetFiles(Name).Length.ToString();
            for (int i = number.Length; i < 4; i++)
            {
                number = "0" + number;
            }
            return Name + "\\" + number + "0.dat";
        }

        private void ExportStructure(byte[] structure, FileStream output)
        {
            output.Write(structure, 0, structure.Length);
        }

        public virtual void Import(string filename)
        {
            using (FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                ImportStructure(infile);
                FileNameCache.Add(filenameCacheNameOverride ?? Name, filename);
            }
        }

        private void ImportStructure(FileStream file)
        {
            rawData = new byte[file.Length];
            file.Read(rawData, 0, (int)file.Length);
        }

        public virtual void Write(Stream outfile)
        {
            outfile.Write(rawData, 0, Size);
        }
    }
}