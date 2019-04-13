namespace GT1.ArchiveExtractor
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