namespace GT2DataSplitter
{
    public class LicenseCar : DataStructure
    {
        public LicenseCar()
        {
            Size = 0x60;
        }

        public override string CreateOutputFilename(byte[] data)
        {
            return Name + "\\" + Utils.GetCarName(data.ReadUInt()) + ".dat";
        }
    }
}
