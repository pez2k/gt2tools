namespace GT1.ArchiveExtractor
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