namespace GT2.GT1ArchiveExtractor
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