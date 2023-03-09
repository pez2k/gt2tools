namespace GT1.DataSplitter
{
    public abstract class Stabilizer : DataStructure
    {
        public Stabilizer()
        {
            Header = "STABILZ";
            Size = 0x14;
        }
    }
}