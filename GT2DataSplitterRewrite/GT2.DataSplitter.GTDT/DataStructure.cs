namespace GT2.DataSplitter.GTDT
{
    public abstract class DataStructure
    {
        public int Size { get; protected set; }

        protected byte[] rawData = [];

        public virtual void Read(Stream infile)
        {
            rawData = new byte[Size];
            infile.Read(rawData, 0, Size);
        }

        public virtual void Write(Stream outfile) => outfile.Write(rawData, 0, Size);
    }
}