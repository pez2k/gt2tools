namespace GT2.GT1ArchiveExtractor
{
    public class USEDCARHeader : KnownHeader
    {
        public USEDCARHeader()
        {
            Extension = "usedcar";
            Header = FromString("@(#)USEDCAR");
        }
    }
}