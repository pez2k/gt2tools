namespace GT1.ArchiveExtractor
{
    public class TIMHeader : KnownHeader
    {
        public TIMHeader()
        {
            Extension = "tim";
            Header = new byte[] { 0x10, 0x00, 0x00, 0x00 };
        }
    }
}