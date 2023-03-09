namespace GT1.DataSplitter
{
    public abstract class Equip : DataStructure
    {
        public Equip()
        {
            Header = "EQUIP";
            Size = 0x6C;
        }
    }
}