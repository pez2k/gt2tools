namespace GT1.ArchiveExtractor
{
    public class PSHeader : KnownHeader
    {
        public PSHeader()
        {
            Extension = "ps";
            Header = FromString("@(#)GT-PS");
        }
    }
}