namespace GT2.GT1ArchiveExtractor
{
    public class BRKCTRLHeader : KnownHeader
    {
        public BRKCTRLHeader()
        {
            Extension = "brkctrl";
            Header = FromString("@(#)BRKCTRL");
        }
    }
}