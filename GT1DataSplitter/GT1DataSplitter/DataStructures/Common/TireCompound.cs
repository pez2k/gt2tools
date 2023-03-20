namespace GT1.DataSplitter
{
    public class TireCompound : DataStructure
    {
        public TireCompound()
        {
            Header = "TIRECMP";
            Size = 0xC8;
            // 0x01: f grip level / Mu
            // 0x65: r grip level / Mu
        }
    }
}